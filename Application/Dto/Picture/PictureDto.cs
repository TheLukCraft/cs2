namespace Application.Dto
{
    public class PictureDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public bool Main { get; set; }
    }
}