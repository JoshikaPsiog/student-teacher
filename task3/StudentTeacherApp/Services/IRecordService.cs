using StudentTeacherApp.DTOs;
using StudentTeacherApp.Models;

namespace StudentTeacherApp.Services
{
    public interface IRecordService
    {
        Task<IEnumerable<Record>> GetAllRecords();
        Task<Record?> GetRecordById(int id);
        Task<Record> CreateRecord(RecordDto dto);
        Task<Record?> UpdateRecord(int id, RecordDto dto);
        Task<bool> DeleteRecord(int id);
    }
}
