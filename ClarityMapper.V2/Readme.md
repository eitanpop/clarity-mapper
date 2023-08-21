# Clarity Care V2 Mapper 

Clarity V2 Mapper houses the functionality to convert HL7 v2 data to FHIR and can be extended to convert to other healthcare formats if need be:
  - HL7v2 to Fhir Service can convert HL7v2 to a list of internal FHIR resource entities 
  - Uses a pipeline to load resource converters to pull relevant data from the parsed HL7 
  - Currenlty uses the Efferent-Health library to parse the HL7 into a Message object 

# Creating a new resource to map

  - Create a new Converter that inherits ```IConverter``` and implements ```ConvertToFhirResource```
  - The ```Extractor``` class helps with validation and querying the HL7 at specific segment and index locations
  - The consumer of the ```HL7v2ToFhirResourcesService``` passes an ```IConverterPipeline```. They must add the new IConverter using the ```Add(IConverter)``` method
  - The HL7 message will pass through all IConverters calling the ```ConvertToFhirResource``` method and each converter will parse or return null if it's not applicable which the ```ConverterPipeline``` filters out.