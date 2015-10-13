using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void submit_Click(object sender, EventArgs e)
    {
        String desc     = descriptionText.Text;
        String loc      = locationText.Text;
        String telno    = telnoText.Text;

        //FileInfo newFile = new FileInfo(@"C:\test.xlsx"); // local path
        FileInfo newFile = new FileInfo(Server.MapPath(@"~/test.xlsx")); // server path

        if (!newFile.Exists)
            createNewExcel(newFile);

        try
        {
            Debug.WriteLine("Starting ...");
            using (ExcelPackage pck = new ExcelPackage(newFile))
            {
                Debug.WriteLine("1 - read workbook");
                ExcelWorkbook wb = pck.Workbook;
                if (wb != null)
                {
                    Debug.WriteLine("Work Book Exist.");
                    if (wb.Worksheets.Count > 0)
                    {
                        ExcelWorksheet currWS = wb.Worksheets.First();
                        //var rowNum = currWS.Cells.Count();
                        int rowNum = Convert.ToInt32(currWS.Dimension.End.Row.ToString());
                        int colNum = Convert.ToInt32(currWS.Dimension.End.Column.ToString());
                        //currWS.Cells[rowNum, colNum].Value = "asdsadaaa";
                        //pck.Save();
                        currWS.Cells[rowNum + 1, 1].Value = desc;
                        currWS.Cells[rowNum + 1, 2].Value = loc;
                        currWS.Cells[rowNum + 1, 3].Value = telno;
                        currWS.Cells[rowNum + 1, 4].Value = "aaaa";
                        Boolean completed = addToLastCell(pck);
                        if (completed)
                            sendEmail();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            throw;
        }
        finally
        {
            Debug.WriteLine("End . . .");
        }
    }

    private Boolean addToLastCell(ExcelPackage pck)
    {
        System.Timers.Timer aTimer = new System.Timers.Timer(600000);
        aTimer.Interval = 60000000;
        aTimer.Stop();
        aTimer.Start();
        
        pck.Save();

        return true;
    }

    private void sendEmail()
    {
        string smtpAddress = System.Configuration.ConfigurationManager.AppSettings["SMTP"];
        int portNumber = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["Port"]);
        bool enableSSL = true;

        string emailFrom = System.Configuration.ConfigurationManager.AppSettings["From"];
        string password = System.Configuration.ConfigurationManager.AppSettings["Pword"];
        string emailTo = System.Configuration.ConfigurationManager.AppSettings["To"];
        string subject = System.Configuration.ConfigurationManager.AppSettings["Subject"];
        string body = System.Configuration.ConfigurationManager.AppSettings["Body"];

        try
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom);
                mail.To.Add(emailTo);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                // Can set to false, if you are sending pure text.

                //mail.Attachments.Add(new Attachment("C:\\SomeFile.txt"));
                //mail.Attachments.Add(new Attachment("C:\\SomeZip.zip"));

                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFrom, password);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            throw;
        }
    }

    private void createNewExcel(FileInfo file)
    {
        ExcelPackage pck = new ExcelPackage(file);
        ExcelWorkbook newWorkbook = pck.Workbook;
        ExcelWorksheet newWorksheet = newWorkbook.Worksheets.Add("Content");
        newWorksheet.Cells[1, 1].Value = "Description";
        newWorksheet.Cells[1, 2].Value = "Location";
        newWorksheet.Cells[1, 3].Value = "TelNo";
        newWorksheet.Cells[1, 4].Value = "Created";
        pck.Save();
    }
}