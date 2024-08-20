namespace TV_Program_Management.Repo
{
    public class SessionStateRepository : IStateRepository
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public SessionStateRepository(IHttpContextAccessor httpContextAccessor) // IHttpContextAccessor للوصول لذاكرة التطبيق، حيث تحتاج لتسجيل أيضاً
        {
            this.httpContextAccessor = httpContextAccessor;
        }



        public string GetValue(string key)
        {
            return httpContextAccessor?.HttpContext?.Session?.GetString(key) ?? string.Empty;

            // OR -->

            //return httpContextAccessor?.HttpContext?.Session?.GetString(key) == null ? String.Empty :
            //    httpContextAccessor.HttpContext.Session.GetString(key);
        }



        public void SetValue(string key, string value)
        {
            httpContextAccessor?.HttpContext?.Session?.SetString(key, value);
        }


        public void Remove(string key)
        {
            httpContextAccessor?.HttpContext?.Session?.Remove(key);
        }
    }
}
