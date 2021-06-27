namespace FakeXiecheng.API.ResourceParameters
{
    public class PaginationResourceParameters
    {
        private int _pageNumber = 1;
        public int PageNumber
        {
            get { return _pageNumber; }
            set
            {
                if (value >= 1)
                {
                    _pageNumber = value;
                }
            }
        }

        private int _pageSize = 10;
        const int MAX_PAGE_SIZE = 50;
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (value >= 1)
                {
                    _pageSize = value > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : value;
                }
            }
        }
    }
}