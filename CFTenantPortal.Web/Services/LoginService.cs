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
            if (String.IsNullOrEmpty(email)) return null;

            // Check if employee
            var employee = await _employeeService.GetByEmailAsync(email);
            if (employee != null)
            {
                if (employee.Active && employee.Password.Equals(password))
                {
                    return employee;
                }
                return null;
            }

            // Check if property owner
            // TODO: Add PropertyOwner.Active
            var propertyOwner = await _propertyOwnerService.GetByEmailAsync(email);
            if (propertyOwner != null) 
            {
                if (!propertyOwner.Password.Equals(password))
                {
                    return propertyOwner;
                }
            }

            return null;
        }      
    }
}
