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
            catch (Exception ex)
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
            catch (Exception ex)
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

        public static List<Config> GetAcademicYears()
        {
            var list = new List<Config>();
            try
            {
                var response = webportals.GetAcademicYears();
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
                    foreach (string item in responseArr)
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
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public static List<OldExam> GetLecturerStudentsOld(string semester, string programme, string unit, string stage)
        {
            var list = new List<OldExam>();
            try
            {
                string response = webportals.GetLecturerStudents(semester, programme, unit, stage);
                if (!string.IsNullOrEmpty(response))
                {
                    int counter = 0;
                    string[] responseArr = response.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in responseArr)
                    {
                        counter++;
                        string[] resultsArr = item.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new OldExam()
                        {
                            TransactionId = resultsArr[0],
                            StudentNo = resultsArr[1],
                            StudentName = resultsArr[2],
                            Counter = counter
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

        public static List<Examination> GetLoStudents(string programme, string unit, string lo)
        {
            var list = new List<Examination>();
            try
            {
                string response = webportals.GetLearningOutcomeStudents(programme, unit, lo);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in responseArr)
                    {
                        string[] resultsArr = item.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Examination()
                        {
                            StudentNo = resultsArr[0],
                            StudentName = resultsArr[1]
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

        public static string GetExamScore(string studentNo, string unit, string semester, string lecturer, string stage, string program)
        {
            string score = string.Empty;
            try
            {
                score = webportals.GetSubmittedExamMarks(studentNo, unit, semester, lecturer, stage, program);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return score;
        }

        public static string GetFormativeExamScore(string studentNo, string unit, string semester, string lecturer, string stage, string program, string learningOutcome, int assessmentCategory)
        {
            string score = string.Empty;
            try
            {
                score = webportals.GetSubmittedFormativeMarks(studentNo, unit, semester, lecturer, stage, program, learningOutcome, assessmentCategory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return score;
        }

        public static string GetMaxScore(string unit, int assessmentCategory)
        {
            string score = string.Empty;
            try
            {
                score = webportals.GetMaxScore(unit, assessmentCategory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return score;
        }

        public static List<Config> GetLoPrograms(string username)
        {
            var list = new List<Config>();
            try
            {
                string response = webportals.GetLoProgrammes(username);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                    foreach (var item in responseArr)
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

        public static List<Config> GetLoStage(string username, string program)
        {
            var list = new List<Config>();
            try
            {
                string response = webportals.GetLoStage(username, program);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                    foreach (var item in responseArr)
                    {
                        list.Add(new Config()
                        {
                            Code = item
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

        public static List<Config> GetLoSemester(string username, string program, string stage)
        {
            var list = new List<Config>();
            try
            {
                string response = webportals.GetLoSemester(username, program, stage);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                    foreach (var item in responseArr)
                    {
                        list.Add(new Config()
                        {
                            Code = item
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

        public static List<Config> GetLoUnits(string username, string program, string stage, string semester)
        {
            var list = new List<Config>();
            try
            {
                string response = webportals.GetLoUnits(username, program, stage, semester);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                    foreach (var item in responseArr)
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

        public static List<Config> GetLoLearningOutcomes(string username, string program, string stage, string semester, string unit)
        {
            var list = new List<Config>();
            try
            {
                string response = webportals.GetLoLearningOutcome(username, program, stage, semester, unit);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                    foreach (var item in responseArr)
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

        public static List<Config> GetAssessmentCategories(string unit)
        {
            var list = new List<Config>();
            try
            {
                string response = webportals.GetAssessmentCategories(unit);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                    foreach (var item in responseArr)
                    {
                        list.Add(new Config()
                        {
                            Code = item
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
    }
}