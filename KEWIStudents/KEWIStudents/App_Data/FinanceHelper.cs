using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KEWIStudents.Models;
using KEWIStudents.NAVWS;

namespace KEWIStudents
{
    public class FinanceHelper
    {
        private static Studentportal webportals = Components.ObjNav;
        private static string[] strLimiters = new string[] { "::" };
        private static string[] strLimiters2 = new string[] { "[]" };

        public static Finance GetStudentFees(string studentNo)
        {
            Finance finance = new Finance();
            try
            {
                string response = webportals.GetStudentFees(studentNo);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    finance.CreditAmount = Convert.ToDecimal(responseArr[0]);
                    finance.DebitAmount = Convert.ToDecimal(responseArr[1]);
                    finance.Balance = Convert.ToDecimal(responseArr[2]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return finance;
        }

        public static List<Finance> GetStudentFeesStatement(string studentNo)
        {
            var list = new List<Finance>();
            try
            {
                string response = webportals.GetFeesStatement(studentNo);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in responseArr)
                    {
                        string[] resultsArr = str.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Finance()
                        {
                            PostingDate = resultsArr[0],
                            DocumentNo = resultsArr[1],
                            Description = resultsArr[2],
                            DebitAmount = Convert.ToDecimal(resultsArr[3]),
                            CreditAmount = Convert.ToDecimal(resultsArr[4])
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

        public static List<Finance> GetPaymentReceipts(string studentNo)
        {
            var list = new List<Finance>();
            try
            {
                string response = webportals.GetPaymentReceipts(studentNo);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in responseArr)
                    {
                        string[] resultsArr = str.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Finance()
                        {
                            PostingDate = resultsArr[0],
                            ReceiptNo=resultsArr[1],
                            PaymentMode=resultsArr[2],
                            StudentName = resultsArr[3],
                            Amount=Convert.ToDecimal(resultsArr[4]),
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