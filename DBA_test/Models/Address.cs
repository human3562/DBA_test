using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBA_test.Models;
public class Address {
    public int Id { get; set; }
    public string House { get; set; }
    public Street AddressStreet { get; set; }
}
