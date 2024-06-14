// Cis.Fiscalization v1.3.0 :: CIS WSDL v1.4 (2012-2017)
// https://github.com/tgrospic/Cis.Fiscalization
// Copyright (c) 2013-present Tomislav Grospic
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Xml.Serialization;

namespace Cis
{
	public static partial class CroatiaFiscalizationInterface
    {

	}

	#region Interfaces
	/// <summary>
	/// Represent request data for CIS service
	/// </summary>
	public interface ICisRequest
	{
		string Id { get; set; }
		SignatureType Signature { get; set; }
	}

	/// <summary>
	/// Represent response data from CIS service
	/// </summary>
	public interface ICisResponse
	{
		GreskaType[] Greske { get; set; }
		ICisRequest Request { get; set; }
	}

	#endregion

	#region FiskalizacijaService partial implementation

	//public partial class FiskalizacijaService
	//{
	//	#region Fields

	//	public bool CheckResponseSignature { get; set; }

	//	#endregion
	//}

	public partial class RacunZahtjev : ICisRequest { }

	public partial class RacunOdgovor : ICisResponse
	{
		[XmlIgnore]
		public ICisRequest Request { get; set; }
	}

	public partial class ProvjeraZahtjev : ICisRequest { }

	public partial class ProvjeraOdgovor : ICisResponse
	{
		[XmlIgnore]
		public ICisRequest Request { get; set; }
	}
	#endregion
}
