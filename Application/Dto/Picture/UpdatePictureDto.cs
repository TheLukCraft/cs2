namespace Application.Dto.Picture
{
    public class UpdatePictureDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public bool Main { get; set; }
    }
}