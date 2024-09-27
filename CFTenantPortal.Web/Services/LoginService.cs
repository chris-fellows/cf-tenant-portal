using CFTenantPortal.Interfaces;

namespace CFTenantPortal.Services
{
    public class LoginService : ILoginService
    {
        private readonly IEmployeeService _employeeService;     
        private readonly IPropertyOwnerService _propertyOwnerService;

        public LoginService(IEmployeeService employeeService,                                 
                                IPropertyOwnerService propertyOwnerService)
        {
            _employeeService = employeeService;            
            _propertyOwnerService = propertyOwnerService;
        }
    
        public async Task<object> AuthenticateAsync(string email, string password)
        {
            const string defaultMessage = "Invalid email or password";

            if (String.IsNullOrEmpty(email)) return defaultMessage;

            // Check if employee
            var employee = await _employeeService.GetByEmailAsync(email);
            if (employee != null)
            {
                if (employee.PasswordReset != null)
                {
                    return "Cannot log in because the password is being reset";
                }
                else if (employee.Active && employee.Password.Equals(password))
                {
                    return employee;
                }
                return defaultMessage;
            }

            // Check if property owner           
            var propertyOwner = await _propertyOwnerService.GetByEmailAsync(email);
            if (propertyOwner != null) 
            {
                if (propertyOwner.PasswordReset != null)
                {
                    return "Cannot log in because the password is being reset";
                }
                else if (!propertyOwner.Password.Equals(password))
                {
                    return propertyOwner;
                }
            }

            return defaultMessage;
        }      
    }
}
