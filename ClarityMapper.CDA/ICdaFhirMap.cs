using System;
using System.Collections.Generic;
using System.Text;

namespace ClarityMapper.CDA
{
    public interface ICdaFhirMap
    {
        IList<CdaFhirMapItem> Generate();
    }
}
