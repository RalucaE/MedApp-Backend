using MedicalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MedicalAPI.Repositories
{
    public class ReportsRepository : IReportsRepository<Report>
    {
        private readonly MedicalAppContext _appContext;
        public ReportsRepository(MedicalAppContext appContext)
        {
            _appContext = appContext;
        }
        List<Report> IReportsRepository<Report>.GetMany(Expression<Func<Report, bool>>? expression)
        {
            try
            {
                if (expression != null)
                {
                    return _appContext.Reports
                         .Include(u => u.User)
                        .Where(expression)
                        .ToList();
                }
                else
                {
                    return _appContext.Reports
                        .Include(u => u.User)
                        .ToList();
                }
            }
            catch
            {
                return new List<Report>();
            }
        }

        Report? IReportsRepository<Report>.Get(int id)
        {
            try
            {
                return _appContext.Reports
                    .FirstOrDefault(u => u.Id == id);
            }
            catch
            {
                return null;
            }
        }

        bool IReportsRepository<Report>.Create(Report report)
        {
            try
            {
                _appContext.Reports.Add(report);
                _appContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        bool IReportsRepository<Report>.Update(Report user)
        {
            throw new NotImplementedException();
        }

        bool IReportsRepository<Report>.Delete(int id)
        {
            try
            {
                Report? existingReport = _appContext.Reports
                    .FirstOrDefault(u => u.Id == id);

                if (existingReport == null)
                    return false;

                _appContext.Reports.Remove(existingReport);
                _appContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        List<Report> IReportsRepository<Report>.GetReportsByUserId(int userId)
        {
            try
            {
                return _appContext.Reports
                    .Include(u => u.User)
                    .Where(r => r.UserId == userId) // Assuming `UserId` is the foreign key in the `Report` entity
                    .ToList();
            }
            catch
            {
                return new List<Report>();
            }
        }     
    }
}
