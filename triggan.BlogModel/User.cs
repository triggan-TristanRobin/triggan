namespace triggan.BlogModel
{
    public class User : Entity
    {
        public string Email { get; set; }
        public string Username { get { return Slug; } set { Slug = value; } }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public bool Active { get; set; }
        public string Role { get; set; }

        public User WithoutPassword()
        {
            var newUser = this.MemberwiseClone() as User;
            newUser.Password = null;
            return newUser;
        }

        public User WithHashedPassword(string hash)
        {
            var newUser = this.MemberwiseClone() as User;
            newUser.Password = hash;
            return newUser;
        }
    }
}
