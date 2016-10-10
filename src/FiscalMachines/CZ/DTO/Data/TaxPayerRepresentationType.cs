namespace FiscalMachines.CZ.DTO.Data
{
    public enum TaxPayerRepresentationType
    {
        DirectRepresentation, // Direct representation without authorization. (přímé zastoupení bez pověření)
        IndirectRepresentation, // Indirect representation (§8 ZoET)
        AuthorizedTransferredRepresentation, // § 9 odst. 1 ZoET
        AuthorizedOtherRepresentation // § 9 odst. 2 ZoET 
    }
}
