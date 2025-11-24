namespace DatingApp.Dtos.Match
{
    public class MatchDto
    {
        public long Id { get; set; }
        public long UserId1 { get; set; }
        public long UserId2 { get; set; }
        public DateTime MatchDate {  get; set; }
    }
}
