namespace GroceryScanner.Domain.Domain
{
    public class PlaceModel
    {
        public List<Prediction> Predictions { get; set; }
        public string Status { get; set; }
    }

    public class Prediction
    {
        public string Description { get; set; }
        public List<MatchedSubstring> MatchedSubstrings { get; set; }
        public string PlaceId { get; set; }
        public string Reference { get; set; }
        public StructuredFormatting StructuredFormatting { get; set; }
        public List<Term> Terms { get; set; }
        public List<string> Types { get; set; }
    }

    public class MatchedSubstring
    {
        public int Length { get; set; }
        public int Offset { get; set; }
    }

    public class StructuredFormatting
    {
        public string MainText { get; set; }
        public List<MatchedSubstring> MainTextMatchedSubstrings { get; set; }
        public string SecondaryText { get; set; }
    }

    public class Term
    {
        public int Offset { get; set; }
        public string Value { get; set; }
    }

}
