namespace EmployeeManagement.Helper
{
    public class PagingHelper
    {
        public string name { get; set; } 
        public string sortBy { get; set; }
        public bool sortByDecending { get; set; } 
        public int page { get; set; } 
        public int pageSize { get; set; } 
    }
}
