﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ODataVersioningSample.Models
{
    /// <summary>
    /// This class initializes the the DbProductsContext database with a good set of initial data.
    /// </summary>
    public class DbProductsContextInitializer : DropCreateDatabaseAlways<DbProductsContext>
    {
        protected override void Seed(DbProductsContext context)
        {
            // NOTE any dates listed below are NOT official.
            // They have simply been copied, probably badly, from Wikipedia.
            DbSupplier microsoft = new DbSupplier
            {
                ID = 1,
                Name = "Microsoft",
                Address = new DbAddress
                {
                    Street = "1 Microsoft Way",
                    City = "Redmond",
                    State = "Washington",
                    ZipCode = "98052",
                    Country = "USA"
                },
                ProductFamilies = new List<DbProductFamily>(){
                    new DbProductFamily { 
                        ID = 1, 
                        Name = "MS-DOS", 
                        Description = "Operating system with a commandline user interface", 
                        Products = new List<DbProduct>(){
                            new DbProduct { Name = "MS-DOS 1.12 (OEM)" },
                            new DbProduct { Name = "MS-DOS 1.19 (OEM)" }, 
                            new DbProduct { Name = "MS-DOS 1.25 (OEM)" },
                            new DbProduct { Name = "MS-DOS 2.0 (OEM)" },
                            new DbProduct { Name = "MS-DOS 2.1 (OEM)" },
                            new DbProduct { Name = "MS-DOS 2.11 (OEM)" },
                            new DbProduct { Name = "MS-DOS 3.0 (OEM)" },
                            new DbProduct { Name = "MS-DOS 3.1 (OEM)" },
                            new DbProduct { Name = "MS-DOS 3.2 (OEM)" },
                            new DbProduct { Name = "MS-DOS 3.21 (OEM)" },
                            new DbProduct { Name = "MS-DOS 3.25 (OEM)" },
                            new DbProduct { Name = "MS-DOS 3.3 (OEM)" },
                            new DbProduct { Name = "MS-DOS 3.3a (OEM)" },
                            new DbProduct { Name = "MS-DOS 3.31 (OEM)" },
                            new DbProduct { Name = "MS-DOS 4.00 (OEM)" },
                            new DbProduct { Name = "MS-DOS 4.01 (OEM)" },
                            new DbProduct { Name = "MS-DOS 4.01a (OEM)" },
                            new DbProduct { Name = "MS-DOS 5.0 (Retail)" },
                            new DbProduct { Name = "MS-DOS 5.0a (Retail)" },
                            new DbProduct { Name = "MS-DOS 5.0.500 (WinNT)" },
                            new DbProduct { Name = "MS-DOS 6.0 (Retail)" },
                            new DbProduct { Name = "MS-DOS 6.1 (Retail)" },
                            new DbProduct { Name = "MS-DOS 6.2 (Retail)" },
                            new DbProduct { Name = "MS-DOS 6.21 (Retail)" },
                            new DbProduct { Name = "MS-DOS 6.22 (Retail)" },         
                            new DbProduct { Name = "MS-DOS 7.0 (Win95, 95A)" },
                            new DbProduct { Name = "MS-DOS 7.1 (Win95B, Win98SE)" },          
                            new DbProduct { Name = "MS-DOS 8.0 (WinME)" }
                        }
                    },
                    new DbProductFamily { 
                        ID = 2, 
                        Name = "Windows", 
                        Description = "Operating System with a graphical user interface",
                        Products = new List<DbProduct>() {
                            new DbProduct { Name = "Windows 1.0", ReleaseDate = new DateTime(1985, 11, 20), SupportedUntil = new DateTime(2001, 12, 31) },
                            new DbProduct { Name = "Windows 2.0", ReleaseDate = new DateTime(1987, 12, 9), SupportedUntil = new DateTime(2001, 12, 31) },
                            new DbProduct { Name = "Windows 2.1x", ReleaseDate = new DateTime(1988, 5, 27), SupportedUntil = new DateTime(2001, 12, 31) },
                            new DbProduct { Name = "Windows 3.0", ReleaseDate = new DateTime(1990, 5, 22), SupportedUntil = new DateTime(2001, 12, 31) },
                            new DbProduct { Name = "Windows 3.1x", ReleaseDate = new DateTime(1992, 4, 6), SupportedUntil = new DateTime(2001, 12, 31) },
                            new DbProduct { Name = "Windows 95", ReleaseDate = new DateTime(1995, 8, 24), SupportedUntil = new DateTime(2001, 12, 31) },
                            new DbProduct { Name = "Windows 98", ReleaseDate = new DateTime(1998, 6, 25), SupportedUntil = new DateTime(2006, 7, 11) },           
                            new DbProduct { Name = "Windows Me", ReleaseDate = new DateTime(2000, 9, 14), SupportedUntil = new DateTime(2006, 7, 11) },
                            new DbProduct { Name = "Windows NT 3.1", ReleaseDate = new DateTime(1993, 7, 27), SupportedUntil = new DateTime(2001, 12, 31) },           
                            new DbProduct { Name = "Windows NT 3.5", ReleaseDate = new DateTime(1994, 9, 21), SupportedUntil = new DateTime(2001, 12, 31) },  
                            new DbProduct { Name = "Windows NT 3.51", ReleaseDate = new DateTime(1995, 5, 30), SupportedUntil = new DateTime(2001, 12, 31) },
                            new DbProduct { Name = "Windows NT 4.0", ReleaseDate = new DateTime(1996, 8, 24), SupportedUntil = new DateTime(2004, 12, 31) },
                            new DbProduct { Name = "Windows 2000", ReleaseDate = new DateTime(2000, 9, 14), SupportedUntil = new DateTime(2006, 7, 11) },
                            new DbProduct { Name = "Windows XP", ReleaseDate = new DateTime(2001, 10, 25), SupportedUntil = new DateTime(2004, 4, 18) },
                            new DbProduct { Name = "Windows Server 2003", ReleaseDate = new DateTime(2003, 4, 24), SupportedUntil = new DateTime(2015, 7, 14) },
                            new DbProduct { Name = "Windows Vista", ReleaseDate = new DateTime(2007, 1, 30), SupportedUntil = new DateTime(2012, 4, 10) },
                            new DbProduct { Name = "Windows Home Server", ReleaseDate = new DateTime(2007, 11, 4), SupportedUntil = new DateTime(2013, 1, 8) },
                            new DbProduct { Name = "Windows Home Server 2011", ReleaseDate = new DateTime(2011, 4, 6), SupportedUntil = new DateTime(2016, 4, 12) },  
                            new DbProduct { Name = "Windows Server 2008", ReleaseDate = new DateTime(2008, 2, 27), SupportedUntil = new DateTime(2018, 7, 10) },
                            new DbProduct { Name = "Windows 7", ReleaseDate = new DateTime(2009, 07, 22), SupportedUntil = new DateTime(2015, 1, 13) },     
                            new DbProduct { Name = "Windows Server 2012" }
                        }
                    },
                    new DbProductFamily { 
                        ID = 3, 
                        Name = "Office", 
                        Description = "Productivity Suite that includes Spreadsheeting and Wordprocessor features",
                        Products = new List<DbProduct>() {
                            new DbProduct { Name = "The Microsoft Office for Windows" },
                            new DbProduct { Name = "The Microsoft Office for Windows 1.5" },
                            new DbProduct { Name = "The Microsoft Office for Windows 1.6" },
                            new DbProduct { Name = "The Microsoft Office for Windows 3.0" },
                            new DbProduct { Name = "Microsoft Office 4.0" },
                            new DbProduct { Name = "Microsoft Office 4.2" },
                            new DbProduct { Name = "Microsoft Office 4.3" },
                            new DbProduct { Name = "Microsoft Office 95" },
                            new DbProduct { Name = "Microsoft Office 97" },
                            new DbProduct { Name = "Microsoft Office 2000" },
                            new DbProduct { Name = "Microsoft Office XP" },
                            new DbProduct { Name = "Microsoft Office 2003" },
                            new DbProduct { Name = "Microsoft Office 2007" },
                            new DbProduct { Name = "Microsoft Office 2010" },
                            new DbProduct { Name = "Microsoft Office 2013" },
                        }
                    }
                }
            };
            context.Suppliers.Add(microsoft);
        }
    }
}