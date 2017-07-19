# simple-thesis

>   a basic LaTeX thesis template, based on [Cambridge PhD template](https://github.com/cambridge/thesis).


## Quick start

1. Clone this repository.
1. Start adding contents of your thesis into [`thesis.tex`](./thesis.tex).
1. Build the `PDF` by running the following in the command line:

    ```
    make
    ```

1. Open `thesis.pdf`.


### Build your thesis

To build the `PDF` version of your thesis, run:

    make

This build procedure uses `pdflatex` and will produce `thesis.pdf`.

To produce `DVI` and `PS` versions of your document, you should run:

    make thesis.ps

This will use the `latex` and `dvips` commands to build the document
and will produce `thesis.dvi` and `thesis.ps` documents.


### Clean unwanted files

To clean unwanted clutter (all LaTeX auto-generated files), run:

    make clean
