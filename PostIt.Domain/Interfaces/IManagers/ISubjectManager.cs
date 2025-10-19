using PostIt.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostIt.Domain.Interfaces.IManagers
{
    public interface ISubjectManager
    {
        List<SubjectModel> GetAllSubjects();

        Task<SubjectModel> CreateSubject(SubjectModel subject);
    }
}
