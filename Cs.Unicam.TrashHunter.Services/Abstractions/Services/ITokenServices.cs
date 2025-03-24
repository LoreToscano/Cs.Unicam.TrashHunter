using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Services.Abstractions.Services
{
    /// <summary>
    /// Interface for Token Services operations.
    /// </summary>
    public interface ITokenServices
    {
        /// <summary>
        /// Generates a token asynchronously based on the provided username and password.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the generated token as a string.</returns>
        public Task<string> GenerateToken(string username, string password);
    }
}