using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KEWIStudents.Models;
using KEWIStudents.NAVWS;

namespace KEWIStudents
{
	public class AcademicsHelper
	{
        private static Studentportal webportals = Components.ObjNav;
        private static string[] strLimiters = new string[] { "::" };
        private static string[] strLimiters2 = new string[] { "[]" };

        public static string StudentStatus(string studentNo)
        {
            string status = string.Empty;
            try
            {
                string studentStatus = webportals.GetStudentStatus(studentNo);
                status=studentStatus;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public static bool IsStudentRegistered()
        {
            bool registered = false;
            try
            {
                string username = HttpContext.Current.Session["username"].ToString();
                registered = webportals.IsStudentRegistered(username);
            }
            catch( Exception ex )
            {
                throw ex;
            }
            return registered;
        }

        public static string[] GetCurrentSemesterDetails()
        {
            string[] semesterDetails = new string[2];
            try
            {
                string details = webportals.GetCurrentSemesterDetails();
                if (!string.IsNullOrEmpty(details))
                {
                    string[] responseArr = details.Split(strLimiters, StringSplitOptions.None);
                    semesterDetails[0] = responseArr[0]; // Registration deadline
                    semesterDetails[1] = responseArr[1]; // Unit registration deadline
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return semesterDetails;
        }

        public static bool SemesterRegistrationPassed()
        {
            bool registrationPassed = false;
            try
            {
                DateTime registrationDeadline = string.IsNullOrEmpty(GetCurrentSemesterDetails()[0])?DateTime.MinValue : Convert.ToDateTime(GetCurrentSemesterDetails()[0]);
                if (DateTime.Now > registrationDeadline) registrationPassed = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return registrationPassed;
        }

        public static List<Config> GetProgramStages(string progId)
        {
            var list = new List<Config>();
            try
            {
                string response = webportals.GetProgramStages(progId);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach(string item in responseArr)
                    {
                        string[] resultArr = item.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Config()
                        {
                            Code = resultArr[0],
                            Description = resultArr[1]
                        });
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return list;
        }
    }
}