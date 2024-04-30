using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DBA_test.Models;
public class Abonent {
    public int Id { get; set; }
    public string FullName { get; set; }
    public Address AbonentAddress { get; set; }
    public PhoneNumber AbonentPhoneNumber { get; set; }    
}

