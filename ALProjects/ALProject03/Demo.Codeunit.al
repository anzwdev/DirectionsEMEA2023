codeunit 50300 "Demo"
{

    procedure ProcessCustomerDocuments(DocumentType: Enum "Sales Document Type"; CustomerNo: Code[20]; UnitPriceChange: Decimal)
    var
        SalesHeader: Record "Sales Header";
        SalesLine: Record "Sales Line";
        MessageText: Text;
    begin
        with SalesHeader do begin
            Reset();
            SetRange("Document Type", DocumentType);
            SetRange("Sell-to Customer No.", CustomerNo);
            if (FindSet()) then
                repeat
                    with SalesLine do begin
                        Reset();
                        SetRange("Document Type", SalesHeader."Document Type");
                        SetRange("Document No.", SalesHeader."No.");
                        if (FindSet(true)) then
                            repeat
                                Validate("Unit Price", "Unit Price" * UnitPriceChange);
                                Modify(true);

                                MessageText := StrSubstNo('%1 %2 %3 %4',
                                    "Sell-to Customer No.",
                                    "Sell-to Customer Name",
                                    "Sell-to Address",
                                    "Sell-to City");

                            until (Next() = 0);
                    end;
                until (Next() = 0);
        end;
    end;
}
