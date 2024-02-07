using MudBlazorDemoBrightsUp.Models;
using static MudBlazor.CategoryTypes;

namespace MudBlazorDemoBrightsUp.DTOs
{
    public class SongDTO
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int totalItemCount { get; set; }
        public int totalPageCount { get; set; }
        public List<SongModel> items { get; set; }
    }
}
