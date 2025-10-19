using PostIt.Domain.Interfaces.IBusiness;
using PostIt.Domain.Interfaces.IManagers;
using PostIt.Domain.Models;
using PostIt.Domain.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostIt.Domain.Business
{
    public class SubjectBusiness : ISubjectBusiness
    {
        private readonly ISubjectManager _subjectManager;

        public SubjectBusiness(ISubjectManager subjectManager)
        {
            _subjectManager = subjectManager;
        }
        public List<SubjectModel> GetAllSubjects()
        {
            return _subjectManager.GetAllSubjects();
        }

        public async Task<SubjectModel> CreateSubject(AddSubjectRequest addSubjectRequest)
        {
            return await _subjectManager.CreateSubject(new SubjectModel 
            { Name = addSubjectRequest.Name, 
                Description = addSubjectRequest.Description }
            );
        }
    }
}
