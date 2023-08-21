# Clarity Care Mapper

Clarity Mapper exposes adapters to parse different formats of legacy data to other formats of legacy data or FHIR. In essence the ClarityMapper project encompasses:
  - Data Adapters to parse legacy healthcare data and returns a parser
  - Parsers to convert to enact the actual conversion to a requested format
  - Expression based mapping from internal domain objects to the firely library (loosely mapped in case we want to use another library)
  - Full test suite + DI enabled

# Mapping from one healthcare format to another

  - Add a ```FromSourceHealthcareName(string source)``` to ```HL7Adapter.cs```
  - The above method needs to call a method on the ```IParserFactory``` so it can return the right parser
  - The ```IParserFactory``` should return a custom parser that inherits from ```IHL7Parser```
  - Ultimately each parser must return a list of Fhir resources (currently Fire.ly) to be converted to Json or XML and sent to the fhir server