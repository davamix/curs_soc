Dataset per el exercici 2: [arbres](https://drive.google.com/open?id=1y2cgoc_V9x8k0qXdBUvrV1ZsKmzkQxMT) (~476 MB)

## Tools
Al directori *tools* hi ha les diferents eines utilitzades per crear el dataset. Han sigut programades en C#.

### Image Downloader
Donada una llista de categories de [Wikimedia](https://commons.wikimedia.org/wiki/Main_Page), descarrega totes les images que hi hagi.

### Rename Files
Canvia el nom original del les imatges descarregades per un nom basat en un nombre.

### Rename Label Index
Canvia el nombre de l'index que hi hagi als arxius de etiquetes per 0. Aquest 0 es l'index de la classe *tree* al fitxer *data/classes.names*

### Custom Files Dataset
Crea els arxius *train.txt* i *test.txt* a partir de les imatges (data/images/) i les etiquetes (data/labels/) corresponents.
