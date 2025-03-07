using AutoMapper;
using CFTenantPortal.Controllers;
using CFTenantPortal.Enums;
using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using CFTenantPortal.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Diagnostics;

namespace CFTenantPortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuditEventService _auditEventService;
        private readonly IAuditEventTypeService _auditEventTypeService;
        private readonly IEmployeeService _employeeService;
        private readonly ILoginService _loginService;
        private readonly IMapper _mapper;        
        private readonly ILogger<HomeController> _logger;
        private readonly IPropertyOwnerService _propertyOwnerService;
        private readonly IRequestInfoService _requestInfoService;
        private readonly ISystemValueTypeService _systemValueTypeService;

        public AccountController(IAuditEventService auditEventService,
                            IAuditEventTypeService auditEventTypeService,
                            IEmployeeService employeeService,
                            ILoginService loginService, IMapper mapper, ILogger<HomeController> logger,
                            IPropertyOwnerService propertyOwnerService,
                            IRequestInfoService requestInfoService,
                            ISystemValueTypeService systemValueTypeService)
        {
            _auditEventService = auditEventService;
            _auditEventTypeService = auditEventTypeService;
            _employeeService = employeeService;
            _loginService = loginService;
            _mapper = mapper;
            _logger = logger;
            _propertyOwnerService = propertyOwnerService;
            _requestInfoService = requestInfoService;
            _systemValueTypeService = systemValueTypeService;
        }

        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Processes form to log out
        /// </summary>
        /// <returns></returns>
        public IActionResult LogoutForm()
        {
            var method = HttpContext.Request.Method;

            if (_requestInfoService.IsLoggedIn)
            {
                // Get employee or property owner
                var userId = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var employee = _employeeService.GetByIdAsync(userId).Result;
                var propertyOwner = employee == null ? _propertyOwnerService.GetByIdAsync(userId).Result : null;

                // Set system value type for user (EmployeeId/PropertyOwnerId)
                SystemValueType userSystemValueType = new();
                if (employee != null)
                {
                    userSystemValueType = _systemValueTypeService.GetByEnum(SystemValueTypes.EmployeeId).Result;
                }
                else if (propertyOwner != null)
                {
                    userSystemValueType = _systemValueTypeService.GetByEnum(SystemValueTypes.PropertyOwnerId).Result;
                }

                // Sign out
                HttpContext.SignOutAsync().Wait();

                //var isLoggedIn = _requestInfoService.IsLoggedIn;

                // Create UserLoggedOut audit event
                var auditEventType = _auditEventTypeService.GetByEnum(AuditEventTypes.UserLoggedOut).Result;
                if (auditEventType != null)
                {
                    var auditEvent = new AuditEvent()
                    {
                        EventTypeId = auditEventType.Id,
                        Parameters = new List<AuditEventParameter>()
                        {
                            new AuditEventParameter()
                            {
                                SystemValueTypeId = userSystemValueType.Id,
                                Value = userId
                            }
                        }
                    };

                    _auditEventService.AddAsync(auditEvent).Wait();
                }
            }

            return Redirect("/");
            //return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Processes form to log in
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public IActionResult LoginForm(LoginVM login)
        {
            var method = HttpContext.Request.Method;

            // Check if logged in            
            if (_requestInfoService.IsLoggedIn)
            {
                return Redirect("/");
                //   return RedirectToAction(nameof(Index));
            }

            // https://stackoverflow.com/questions/73748488/passing-username-and-password-to-controller-method-asp-net-mvc
            // https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-8.0
            if (ModelState.IsValid)
            {
                // Check email & password
                var authenticateResult = _loginService.AuthenticateAsync(login.Email, login.Password).Result;
                if (authenticateResult is string)   // Failure (Invalid credentials, password reset active)
                {
                    ViewBag.Message = (string)authenticateResult;
                }
                else
                {
                    var claims = new List<Claim>();
                    SystemValueType userSystemValueType = new();    // EmployeeId/PropertyOwnerId

                    if (authenticateResult is Employee)
                    {
                        userSystemValueType = _systemValueTypeService.GetByEnum(SystemValueTypes.EmployeeId).Result;

                        var employee = (Employee)authenticateResult;
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, Convert.ToString(employee.Id)));
                        claims.Add(new Claim(ClaimTypes.Name, employee.Name));
                        if (employee.Roles != null)
                        {
                            claims.AddRange(employee.Roles.Select(role => new Claim(ClaimTypes.Role, role.ToString())));
                        }

                        /*
                        var claims = new List<Claim>() {
                            new Claim(ClaimTypes.NameIdentifier, Convert.ToString(employee.Id)),
                                new Claim(ClaimTypes.Name, employee.Name)
                        };
                        */
                        //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                        //{
                        //    IsPersistent = user.RememberLogin
                        //});
                        //return LocalRedirect(user.ReturnUrl);                     
                    }
                    else if (authenticateResult is PropertyOwner)
                    {
                        userSystemValueType = _systemValueTypeService.GetByEnum(SystemValueTypes.PropertyOwnerId).Result;

                        var propertyOwner = (PropertyOwner)authenticateResult;
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, Convert.ToString(propertyOwner.Id)));
                        claims.Add(new Claim(ClaimTypes.Name, propertyOwner.Name));
                        claims.Add(new Claim(ClaimTypes.Role, UserRoles.PropertyOwner.ToString())); // TODO: Add PropertyOwner.Roles
                        //if (propertyOwner.Roles != null)
                        //{
                        //    claims.AddRange(propertyOwner.Roles.Select(role => new Claim(ClaimTypes.Role, role.ToString())));
                        //}
                    }

                    // Create claims principle
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    // Sign in
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                    {
                        IsPersistent = true
                    }).Wait();

                    // Create UserLoggedIn audit event
                    var auditEventType = _auditEventTypeService.GetByEnum(AuditEventTypes.UserLoggedIn).Result;
                    if (auditEventType != null)
                    {
                        var auditEvent = new AuditEvent()
                        {
                            EventTypeId = auditEventType.Id,
                            Parameters = new List<AuditEventParameter>()
                                {
                                    new AuditEventParameter()
                                    {
                                        SystemValueTypeId = userSystemValueType.Id,
                                        Value = claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value
                                    }
                                }
                        };

                        _auditEventService.AddAsync(auditEvent).Wait();
                    }

                    return Redirect("/");
                    //return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Login));
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult ResetPasswordSent()
        {
            return View();
        }

        /// <summary>
        /// Processes form to reset password
        /// </summary>
        /// <param name="forgotPassword"></param>
        /// <returns></returns>
        public IActionResult ForgotPasswordForm(ForgotPasswordVM forgotPassword)
        {
            // Get employee or property owner
            var employee = _employeeService.GetByEmailAsync(forgotPassword.Email).Result;
            var propertyOwner = (employee == null ? _propertyOwnerService.GetByEmailAsync(forgotPassword.Email).Result : null);

            var passwordResetId = Guid.NewGuid().ToString();

            // TODO: Send email
            if (employee != null || propertyOwner != null)
            {

            }

            // Update employee or property owner to indicate password reset active
            if (employee != null)
            {
                employee.PasswordReset = new PasswordReset()
                {
                    Id = passwordResetId,
                    EmailSendDateTime = DateTimeOffset.UtcNow,
                    ExpiryDateTime = DateTimeOffset.UtcNow.AddMinutes(120)
                };

                _employeeService.UpdateAsync(employee).Wait();
            }
            else if (propertyOwner != null)
            {
                propertyOwner.PasswordReset = new PasswordReset()
                {
                    Id = passwordResetId,
                    EmailSendDateTime = DateTimeOffset.UtcNow,
                    ExpiryDateTime = DateTimeOffset.UtcNow.AddMinutes(120)
                };

                _propertyOwnerService.UpdateAsync(propertyOwner).Wait();
            }
            return RedirectToAction(nameof(ResetPasswordSent));
        }

        /// <summary>
        /// Handles user clicking on 'Reset password' link in email
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult ResetPasswordFromEmail(string id)
        {
            // TODO: Load Employee or PropertyOwner from reset Id

            var employee = _employeeService.GetByIdAsync(id).Result;
            var propertyOwner = (employee == null ? _propertyOwnerService.GetByIdAsync(id).Result : null);

            if (employee != null)
            {
                if (employee.PasswordReset == null)   // No active reset
                {

                }
                else if (employee.PasswordReset.Id == id)   // User clicked on link in latest reset email
                {
                    return RedirectToAction(nameof(ResetPassword), new { id = id });
                }
                else    // User clicked on link in old reset email
                {

                }
            }
            else if (propertyOwner != null)
            {
                if (propertyOwner.PasswordReset == null)    // No active reset
                {

                }
                else if (propertyOwner.PasswordReset.Id == id)   // User clicked on link in latest reset email
                {
                    return RedirectToAction(nameof(ResetPassword), new { id = id });
                }
                else    // User clicked on link in old reset email
                {

                }
            }

            return RedirectToAction(nameof(ResetPassword), new { id = id });
        }

        public IActionResult ResetPassword(string id)
        {
            return View();
        }

        /// <summary>
        /// Processes form to reset password
        /// </summary>
        /// <param name="resetPassword"></param>
        /// <returns></returns>
        public IActionResult ResetPasswordForm(ResetPasswordVM resetPassword)
        {
            // Get employee or property owner
            var employee = _employeeService.GetByIdAsync(resetPassword.UserId).Result;
            var propertyOwner = (employee == null ? _propertyOwnerService.GetByIdAsync(resetPassword.UserId).Result : null);

            if (employee != null)
            {
                // Check that reset hasn't expired
                if (employee.PasswordReset!.ExpiryDateTime <= DateTimeOffset.UtcNow)
                {

                }

                employee.Password = resetPassword.Password1;
                employee.PasswordReset = null;

                _employeeService.UpdateAsync(employee).Wait();
            }
            else if (propertyOwner != null)
            {
                // Check that reset hasn't expired
                if (propertyOwner.PasswordReset!.ExpiryDateTime <= DateTimeOffset.UtcNow)
                {

                }

                propertyOwner.Password = resetPassword.Password1;
                propertyOwner.PasswordReset = null;

                _propertyOwnerService.UpdateAsync(propertyOwner).Wait();
            }

            return Redirect("/");
            //return RedirectToAction(nameof(Index));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
