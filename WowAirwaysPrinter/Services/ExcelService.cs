using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WowAirwaysPrinter.Models;

namespace WowAirwaysPrinter.Services
{
    public class ExcelService
    {
        private List<Attendee> _attendees;

        public ExcelService()
        {
            _attendees = null;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public List<Attendee> Read(string filename)
        {
            // Read Excel file
            using (var package = new ExcelPackage(new FileInfo(filename)))
            {
                _attendees = new List<Attendee>();

                // Assume you are working with the first worksheet (index 1)
                var worksheet = package.Workbook.Worksheets["SEATING"];

                // Get the number of rows and columns in the worksheet
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                // Iterate through rows and columns to read cell values
                for (int row = 2; row <= rowCount; row++)
                {
                    _attendees.Add(new Attendee()
                    {
                        FinalSeating = worksheet.Cells[row, 4].Text,
                        FullName = worksheet.Cells[row, 5].Text,
                        Division = worksheet.Cells[row, 6].Text.Trim()
                    });

                }
            }
            return _attendees;

        }
    }
}
