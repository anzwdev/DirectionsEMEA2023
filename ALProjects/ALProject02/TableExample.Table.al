table 50301 "Table Example"
{
    DataClassification = CustomerContent;

    fields
    {
        field(1; "No."; Code[20])
        {
        }
        field(2; Name; Text[100])
        {
        }
        field(3; "Name 2"; Text[100])
        {
        }
    }

    keys
    {
        key(Key1; "No.")
        {
            Clustered = true;
        }
    }
}
