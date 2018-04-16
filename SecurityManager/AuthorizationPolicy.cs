using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityManager
{
    public enum Permission
    {
            LoginAdministracija,
            LoginSkladistenje,
            LoginRacunovodstvo,
            ResetPassword,
            AddRoll,
            EditRoll,
            DeleteRoll,
            AddUser,
            EditUser,
            DeleteUser,
            AddSkladiste,
            DeleteSkladiste,
            EditSkladiste,
            AddProizvod,
            DeleteProizvod,
            EditProizvod,
            AddProizvodjac,
            DeleteProizvodjac,
            EditProizvodjac,
            AddGrad,
            DeleteGrad,
            EditGrad,
            AddUlazna,
            EditUlazna,
            DeleteUlazna,
            AddIzlazna,
            EditIzlazna,
            DeleteIzlazna,
            AddStorno,
            EditStorno,
            DeleteStoro,
            AddProfaktura,
            EditProfaktura,
            DeleteProfaktura,
            EditZaposleniSkl
    }

    public class AuthorizationPolicy
    {
        private static DeltaEximEntities dbContext = new DeltaEximEntities();

        public static bool HavePermission(int korisnik_id, Permission p)
        {
            
            string permission = convertToString(p);
            if (dbContext.Korisniks.Any(x => x.id == korisnik_id && x.active == true))
            {
                if (p.Equals(Permission.LoginAdministracija) || p.Equals(Permission.LoginRacunovodstvo) || p.Equals(Permission.LoginSkladistenje) || p.Equals(Permission.ResetPassword))
                {
                    if (dbContext.Korisniks.Any(x => x.id == korisnik_id && x.ulogovan == true))
                    {
                        return false;
                    }
                }
                else
                {
                    if (dbContext.Korisniks.Any(x => x.id == korisnik_id && x.ulogovan == false))
                    {
                        return false;
                    }
                }


                if (dbContext.Permissions.Any(x => x.naziv.Equals(permission)))
                {
                    int id_zaposleni = dbContext.Korisniks.First(x => x.id == korisnik_id && x.active == true).zaposleni_id;
                    if (dbContext.Zaposlenis.Any(x => x.id == id_zaposleni && x.active == true))
                    {
                        ICollection<Common.Model.Uloga> uloge = dbContext.Zaposlenis.First(x => x.id == id_zaposleni && x.active == true).Ulogas;
                        foreach (Uloga u in uloge)
                        {
                            if (u.Permissions.Any(x => x.naziv.Equals(permission)))
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static String convertToString(Permission p)
        {
            return Permission.GetName(p.GetType(), p);
        }
    }
}
