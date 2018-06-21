using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EMSMainService.BusinessEntities;
using System.Web.Http.Cors;

namespace EMSMainService.Controllers
{
    [EnableCors("*", "*", "*")]
    [Authorize]
    public class EmployeeController : ApiController
    {

        //GET Employees
        [Route("api/employees")]
        [HttpGet]
        public HttpResponseMessage GetEmployees()
        {
            try
            {
                EmployeeDBEntities1 context = new EmployeeDBEntities1();
                var employees = context.Employees.ToList();
                List<EmployeeModel> list = new List<EmployeeModel>();
                foreach (var item in employees)
                {
                    EmployeeModel emp = new EmployeeModel();
                    emp.Id = item.Id;
                    emp.FirstName = item.FirstName;
                    emp.LastName = item.LastName;
                    emp.Email = item.Email;
                    emp.Gender = item.Gender;
                    emp.Country = item.Country.Name;
                    emp.State = item.State.Name;
                    emp.City = item.City.Name;
                    emp.Department = item.Department.Name;
                    emp.DateOfBirth = item.DateOfBirth;
                    emp.DateOfJoining = item.DateOfJoining;
                    emp.IsActive = item.IsActive;
                    emp.Role = item.RoleMaster.Name;
                    list.Add(emp);
                }
                return Request.CreateResponse((employees != null && employees.Count > 0) ? HttpStatusCode.OK : HttpStatusCode.NoContent, list);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex.Message);
            }
        }

        [Route("api/create")]
        [HttpPost]
        public HttpResponseMessage CreateEmployee(EmployeeModel employeeModel)
        {
            try
            {
                EmployeeDBEntities1 context = new EmployeeDBEntities1();
                Employee employee = new Employee();
                employee.Id = 0;
                employee.FirstName = employeeModel.FirstName;
                employee.LastName = employeeModel.LastName;
                employee.Gender = employeeModel.Gender;
                employee.Email = employeeModel.Email;
                employee.Phone = employeeModel.Phone;
                employee.IsActive = true;
                employee.DateOfBirth = employeeModel.DateOfBirth;
                employee.DateOfJoining = employeeModel.DateOfJoining;
                employee.CountryId = employeeModel.CountryId;
                employee.StateId = employeeModel.StateId;
                employee.CityId = employeeModel.CityId;
                employee.DepartmentId = employeeModel.DepartmentId;
                employee.RoleType = employeeModel.RoleId;
                var employees = context.Employees.Add(employee);
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("api/employee/{id}")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeById(int id)
        {
            try
            {
                EmployeeDBEntities1 context = new EmployeeDBEntities1();
                var employees = context.Employees.Where(emp=>emp.Id == id).FirstOrDefault();
                if (employees == null)
                    return Request.CreateResponse(HttpStatusCode.NoContent, string.Format("No Employee with Id = {0} found", id));
                else
                {
                    EmployeeModel employee = new EmployeeModel();
                    employee.Id = employees.Id;
                    employee.FirstName = employees.FirstName;
                    employee.LastName = employees.LastName;
                    employee.Email = employees.Email;
                    employee.Gender = employees.Gender;
                    employee.Phone = employees.Phone;
                    employee.Country = employees.Country.Name;
                    
                    employee.State = employees.State.Name;
                    employee.City = employees.City.Name;
                    employee.Department = employees.Department.Name;
                    employee.DateOfBirth = employees.DateOfBirth;
                    employee.DateOfJoining = employees.DateOfJoining;
                    employee.IsActive = employees.IsActive;
                    employee.Role = employees.RoleMaster.Name;
                    return Request.CreateResponse(HttpStatusCode.OK, employee);
                    //   return Request.CreateResponse(HttpStatusCode.NoContent);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("api/employee/countries")]
        [HttpGet]
        public HttpResponseMessage GetCountries()
        {
            try
            {
                EmployeeDBEntities1 context = new EmployeeDBEntities1();
                var countries = context.Countries.ToList();
                return Request.CreateResponse(HttpStatusCode.OK,countries);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("api/employee/departments")]
        [HttpGet]
        public HttpResponseMessage GetDepartments()
        {
            try
            {
                EmployeeDBEntities1 context = new EmployeeDBEntities1();
                var countries = context.Departments.ToList();
                return Request.CreateResponse(HttpStatusCode.OK, countries);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("api/employee/roles")]
        [HttpGet]
        public HttpResponseMessage GetRoles()
        {
            try
            {
                EmployeeDBEntities1 context = new EmployeeDBEntities1();
                var countries = context.RoleMasters.ToList();
                return Request.CreateResponse(HttpStatusCode.OK, countries);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("api/employee/states/{countryId}")]
        [HttpGet]
        public HttpResponseMessage GetStates(int countryId)
        {
            try
            {
                EmployeeDBEntities1 context = new EmployeeDBEntities1();
                var states = context.States.Where(x => x.CountryId == countryId).ToList();
                List<StateModel> lst = new List<StateModel>();
                if (states != null && states.Count > 0)
                {
                    foreach (var item in states)
                    {
                        StateModel sm = new StateModel() { Id = item.Id, Name = item.Name };
                        lst.Add(sm);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, lst);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("api/employee/cities/{stateId}")]
        [HttpGet]
        public HttpResponseMessage GetCities(int stateId)
        {
            try
            {
                EmployeeDBEntities1 context = new EmployeeDBEntities1();
                var cities = context.Cities.Where(x => x.StateId == stateId).ToList();
                List<CityModel> lst = new List<CityModel>();
                if (cities != null && cities.Count > 0)
                {
                    foreach (var item in cities)
                    {
                        CityModel sm = new CityModel() { Id = item.Id, Name = item.Name };
                        lst.Add(sm);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, lst);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



    }

    public class EmployeeModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public int Phone { get; set; }
        public string Country { get; set; }
        public int CountryId { get; set; }
        public string State { get; set; }
        public int StateId { get; set; }
        public string City { get; set; }
        public string Department { get; set; }
        public int DepartmentId { get; set; }
        public int CityId { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public System.DateTime DateOfJoining { get; set; }
        public bool IsActive { get; set; }
        public string Role { get; set; }
        public int RoleId { get; set; }
    }

    public class StateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CityModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
