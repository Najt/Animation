// <copyright file="IAnimated.cs" company="GamerCats Studios">
//     GamerCats Studios All rights reserved
// </copyright>
// <author>Duna Gergely Endre</author>
namespace Game1
{
    /// <summary>
    /// Interface for animated Assets
    /// </summary>
    public interface IAnimated
    {
        #region Properties

        /// <summary>
        /// Gets the scene Which contains the animated asset
        /// </summary>
        Scene ParentScene { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Method which updates the asset's animations
        /// </summary>
        /// <param name="elapsedTime">Elapsed time since last call in milliseconds</param>
        void UpdateAnimations(float elapsedTime);

        #endregion Methods
    }
}