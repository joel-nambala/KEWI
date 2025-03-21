using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KEWIStaff.Models;
using KEWIStaff.NAVWS;

namespace KEWIStaff
{
	public class ExamHelper
	{
		private static Staffportal webportals = Components.ObjNav;
		private static string[] strLimiters = new string[] { "::" };
		private static string[] strLimiters2 = new string[] { "[]" };

		public static List<Config> GetLecturerProgrammes(string username)
		{
			var list = new List<Config>();
			try
			{
				string response = webportals.GetLecturerProgrammes(username);
				if (!string.IsNullOrEmpty(response))
				{
					string[] responseArr = response.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                    foreach (string item in responseArr)
					{
						string[] resultsArr = item.Split(strLimiters, StringSplitOptions.None);
						list.Add(new Config()
						{
							Code = resultsArr[0],
							Description = resultsArr[1]
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

		public static List<Config> GetLecturerStages(string username, string programme)
		{
			var list = new List<Config>();
			try
			{
				var response = webportals.GetLecturerStages(username, programme);
				if (!string.IsNullOrEmpty(response))
				{
					string[] responseArr = response.Split(strLimiters, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                    foreach (string item in responseArr)
                    {                        
                        list.Add(new Config()
                        {
                            Code = item,
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

        public static List<Config> GetLecturerSemesters(string username, string programme, string stage)
        {
            var list = new List<Config>();
            try
            {
                var response = webportals.GetLecturerSemester(username, programme, stage);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                    foreach (string item in responseArr)
                    {
                        list.Add(new Config()
                        {
                            Code = item,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public static List<Config> GetLecturerUnits(string username, string programme, string stage, string semester)
        {
            var list = new List<Config>();
            try
            {
                var response = webportals.GetLecturerUnits(username, programme, stage, semester);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                    foreach (string item in responseArr)
                    {
                        string[] resultsArr = item.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Config()
                        {
                            Code = resultsArr[0],
                            Description = resultsArr[1]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public static List<Config> GetCampus()
        {
            var list = new List<Config>();
            try
            {
                var response = webportals.GetCampus();
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in responseArr)
                    {
                        string[] resultsArr = item.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Config()
                        {
                            Code = resultsArr[0],
                            Description = resultsArr[1]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public static List<Examination> GetLecturerStudents(string semester, string programme, string unit, string stage)
        {
            var list = new List<Examination>();
            try
            {
                string response = webportals.GetLecturerStudents(semester, programme, unit, stage);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach(string item in responseArr)
                    {
                        string[] resultsArr = item.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Examination()
                        {
                            TransactionId = resultsArr[0],
                            StudentNo = resultsArr[1],
                            StudentName = resultsArr[2]
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

        public static string GetExamScore(string studentNo, string unit, string semester, string lecturer, string stage, string program)
        {
            string score = string.Empty;
            try
            {
                score = webportals.GetSubmittedExamMarks(studentNo, unit, semester, lecturer, stage, program);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return score;
        }
    }
}