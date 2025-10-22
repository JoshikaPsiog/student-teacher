using StudentTeacherApp.Data;
using StudentTeacherApp.DTOs;
using StudentTeacherApp.Models;
using Microsoft.EntityFrameworkCore;

namespace StudentTeacherApp.Services
{
    public class RecordService : IRecordService
    {
        private readonly ApplicationDbContext _context;

        public RecordService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Record>> GetAllRecords()
        {
            return await _context.Records.ToListAsync();
        }

        public async Task<Record?> GetRecordById(int id)
        {
            return await _context.Records.FindAsync(id);
        }

        public async Task<Record> CreateRecord(RecordDto dto)
        {
            var record = new Record
            {
                Title = dto.Title,
                Description = dto.Description
            };

            _context.Records.Add(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<Record?> UpdateRecord(int id, RecordDto dto)
        {
            var record = await _context.Records.FindAsync(id);
            if (record == null) return null;

            record.Title = dto.Title;
            record.Description = dto.Description;

            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<bool> DeleteRecord(int id)
        {
            var record = await _context.Records.FindAsync(id);
            if (record == null) return false;

            _context.Records.Remove(record);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
