namespace Karl.Settings
{
    struct Setting
    {
        public string FileName;
        public string Name;

        public object Value;
        public object Default;

        public ValidatorDelegate Validate;
        public SaveFormatDelegate SaveFormat;

        public string AsString()
        {
            return Value != null ? (string) Value : (string) Default;
        }

        public bool AsBool()
        {
            return Value != null ? (bool)Value : (bool)Default;
        }

        public int AsInt()
        {
            return Value != null ? (int)Value : (int)Default;
        }

        public float AsFloat()
        {
            return Value != null ? (float) Value : (float) Default;
        }
    }
}
