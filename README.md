# GameObject Picker

A CustomPropertyDrawer for GameObjects that extends the default PropertyDrawer with a convenient eyedropper-button, which, instead of having to drag a GameObject from the hierarchy into the field, allows editor users to pick one directly from the scene view.

## Requirements

- Unity 2019.3 or newer

## Installing

Installation happens through Unity's Package Manager. Open the Package Manager -window, click the plus-icon in the top-left corner and select 'Add package from git URL'. In the field, type:
https://github.com/EngineArtist/GameObjectPicker.git
Now the package should show up in the list of packages. Click it and click the Install-button in the bottom-right corner.

## Usage

Once the package has been installed, all GameObject-fields in the UnityEditor should now show an eyedropper-button next to them. When a GameObject-fields' eyedropper is clicked, you can now click a GameObject in the scene view to assign that GameObject to the field. This is the same as dragging the GameObject from the Hierarchy to the field, but the eyedropper offers a more convenient way in cases where you have lots of GameObjects with the same name that are difficult to tell apart in the Hierarchy.

## Author

Miika Vihersaari <miika.vihersaari@gmail.com>

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## Acknowledgements

- User streeetwalker on the Unity forums helped me find a bug, thank you!
- Thanks to Unity for making a great engine and editor!