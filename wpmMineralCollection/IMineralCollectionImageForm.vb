Imports WebProjectMechanics
Imports wpmMineralCollection


Public Interface IMineralCollectionImageForm
    Sub SetSpecimenImage(ByVal mySpecimenImage As SpecimenImage)
    Function GetSpecimenImage() As SpecimenImage
End Interface
