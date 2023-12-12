using Firebase.Auth.Provider;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http.HttpResults;
using ResenaApi.Modelos;
using ResenaApi.Services.Interfaces;

namespace ResenaApi.Services
{
    public class ServiceAuth: IServiceAuth
    {
        private FirebaseAuth _firebaseAuth;
        private FirebaseAuthProvider _firebaseAuthProvider;

        private IServicePersonas _servicePersonas;

      


        public ServiceAuth(FirebaseAuth firebaseAuth, FirebaseAuthProvider firebaseAuthProvider, IServicePersonas servicePersonas)
        {


            _firebaseAuth = firebaseAuth;
            _firebaseAuthProvider = firebaseAuthProvider;
            _servicePersonas = servicePersonas;
        }

        public async Task<string> Register(Auth auth, Personas personas)
        {
            try
            {
                var user = await _firebaseAuth.CreateUserAsync(new UserRecordArgs
                {
                    Email = auth.email,
                    Password = auth.password
                });
                
                var persona = await _servicePersonas.CrearPersona(personas);

                if (persona != null)
                {
                    return user.Uid;
                }
                else
                {
                    var eliminar = await Eliminar(user.Uid);
                    if (eliminar != null)
                    {
                        return "no se pudo crear";
                    }

                    return "conflicto a eliminar uid de persona no creada";
                }

               
                
            }
            catch (FirebaseAuthException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> Login(Auth auth)
        {
            try
            {
                var signInResult = await _firebaseAuthProvider.SignInWithEmailAndPasswordAsync(auth.email, auth.password);
                return signInResult.User.LocalId;

            }
            catch (FirebaseAuthException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> ForgotPassword(string email)
        {
            try
            {
                await _firebaseAuthProvider.SendPasswordResetEmailAsync(email);
                return "Password reset email sent successfully";
            }
            catch (FirebaseAuthException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> SuspendUser(string uid)
        {
            try
            {
                var user = await _firebaseAuth.GetUserAsync(uid);
                if (user != null)
                {
                    await _firebaseAuth.UpdateUserAsync(new UserRecordArgs
                    {
                        Uid = uid,
                        Disabled = true
                    });

                    return "User suspended successfully";
                }

                return "User not found";
            }
            catch (FirebaseAuthException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> Eliminar(string uid)
        {
            try
            {
                var user = await _firebaseAuth.GetUserAsync(uid);
                if (user != null)
                {
                    await _firebaseAuth.DeleteUserAsync(uid);
                    return "User deleted successfully";
                }

                return "User not found";
            }
            catch (FirebaseAuthException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> HabilitarUser(string uid)
        {
            try
            {
                var user = await _firebaseAuth.GetUserAsync(uid);
                if (user != null)
                {
                    await _firebaseAuth.UpdateUserAsync(new UserRecordArgs
                    {
                        Uid = uid,
                        Disabled = false
                    });

                    return "User enabled successfully";
                }

                return "User not found";
            }
            catch (FirebaseAuthException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

