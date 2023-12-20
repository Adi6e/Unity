
public static class WordsHandler {
    [System.Serializable]
    public class WordObject {
        public string Id;
        public string Language;
        public string TextSeq;
        public int Points;
        public string PrefabName;
    }

    [System.Serializable]
    public class WordsList {
        public WordObject[] words;
    }
    public static WordsList Words = new WordsList();
}
