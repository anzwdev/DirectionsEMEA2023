table 50301 "Table Example"
{
    DataClassification = CustomerContent;

    fields
    {
        field(1; "No."; Code[20])
        {
            Caption = 'No.';
        }
        //Field comment

#pragma warning disable AA123

        field(2; Name; Text[100])
        {
            Caption = 'Name';
        }

#pragma warning restore AA123

        field(3; "Name 2"; Text[100])
        {
            Caption = 'Name 2';
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
