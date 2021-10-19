namespace OOP_WORKSHOP_PROJECT.Authorization{
    public interface IAuthorize{

        //Generate a token for a specific id
        public string Generate(int id);

        //Get userId from a token
        public int GetUserId(string token);
    }
}