namespace DatingApp.Dtos.Hobbie
{
    public class EditHobbieRequest
    {
        public long Id { get; set; }

        public string HobbieName { get; set; } = default!;
    }
}
