%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%
%% Class ``simple-thesis''
%%
%% Simplified version of https://github.com/cambridge/thesis, by
%% Tom Gurion (nagasaki45).
%%
%% Original template details:
%%
%% Version: v0.2
%% Authors: Jean Martina, Rok Strnisa, Matej Urbas
%% Date: 30/07/2008
%%
%% Copyright (c) 2008-2012, Rok Strniša, Jean Martina, Matej Urbas
%% License: Simplified BSD License
%% License file: ./License
%% Original License URL: http://www.freebsd.org/copyright/freebsd-license.html
%%%%%


%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%
%% Class identification.
%%
%%%%%
\NeedsTeXFormat{LaTeX2e}
\ProvidesClass{simple-thesis}[2017/07/06 My simple thesis LaTeX template]

\typeout{}
\typeout{***********************************************}
\typeout{***********************************************}
\typeout{}


%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%
%% Using report class as base.
%%
%%%%%
\PassOptionsToClass{a4paper,11pt}{report}
\DeclareOption*{\PassOptionsToClass{\CurrentOption}{report}}
\ProcessOptions\relax
\LoadClass{report}


%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%
%% Additional packages, and their options.
%%
%%%%%
\RequirePackage{graphicx}           % Required for the affiliation Logo (on the title page)
\RequirePackage{calc}               % Used for calculating margins and laying out the title page
\RequirePackage{setspace}      	    % Used for selecting single or onehalf line spacing.
\RequirePackage[round]{natbib}      % We need to generate bibliography

% Hyperref should be included in any case, but we make it non-obtrusive (i.e.: no hyperlink borders).
\RequirePackage[
    pdffitwindow=true,
    pdfpagelabels=true,
    colorlinks=false,
    pdfborder={0 0 0},
    pdfusetitle=true
]{hyperref}
\RequirePackage[all]{hypcap}        % Fixes links to floats (must be included after hyperref).


%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%
%% Page margins (suitable for J.S. Wilson & Son).
%%
%%%%%

% Setting the offsets to 'zero' (LaTeX calculates offsets 1in from the left and upper edges of the page) makes the calculation of margins a bit easier.
\setlength{\hoffset}{-1in}
\setlength{\voffset}{-1in}

\newlength{\simple@topmargin}
\newlength{\simple@bottommargin}


%% Calculate and set the margins properly (with parameters that actually have
%% some meaning for everyday thesis-writers).
%%
%% @param	1	odd side margin (inner margin).
%% @param	2	even side margin (outer margin).
%% @param	3	top margin.
%% @param	4	bottom margin.
\DeclareRobustCommand{\simple@calcpaperdims}[4]{%
% MARGINS
% 'Top margin' is the distance between the top of the text and the top of the page.
% 'Bottom margin' is the distance between the bottom of the footer (the page number) and the bottom of the page.
\setlength{\oddsidemargin}{#1}        % inner margin
\setlength{\evensidemargin}{#2}       % outer margin
\setlength{\simple@topmargin}{#3}        % top margin        (the distance from the top of the page to the top of the body text -- the header is located between)
\setlength{\simple@bottommargin}{#4}     % bottom margin     (the distance from the bottom of the page to the bottom of the body text -- the footer is located between)
% Horizontal spacing
\setlength{\textwidth}{\paperwidth-#1-#2}     % text takes the remaining width (210 - inner - outer)

\setlength{\marginparwidth}{\evensidemargin-8mm} % the margin only has 'outer' space available, so we have to make it a bit thinner.
\setlength{\marginparsep}{3mm}

% Vertical spacing
\setlength{\headheight}{5mm}      % The height of the box where the heading text lives
\setlength{\headsep}{5mm}         % The distance between the heading and the top of the text
\setlength{\topmargin}{#3-\headheight-\headsep}

\setlength{\textheight}{\paperheight-#3-1.7\simple@bottommargin}   % text takes the remaining height (297 - top margin - bottom margin)
\setlength{\footskip}{.7\simple@bottommargin} % The distance from the bottom of the text to the bottom of the footer
}
\simple@calcpaperdims{30mm}{20mm}{20mm}{20mm}


%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%
%% Line spacing
%%
%%%%%

% One-and-a-half spacing (required by board of graduate studies).
% (Should remove this for Computer Laboratory technical report.)
\onehalfspacing%


%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%
%% Variable definitions and default values: these variables should be defined by
%% the user (somewhere in the preamble). For example, to put the abstract into
%% the thesis, the thesis writer should type the following somewhere in the
%% preamble (before the `\begin{document}` or `\frontmatter` commands are
%% called):
%%
%%      \abstract{This is my abstract.}
%%
%% See below (in the comments starting with 'DOCVAR: ') for a list of all
%% variables the thesis writer is expected to use.
%%
%%%%%

% DOCVAR: abstract (The text that will be instered into the abstract of the
% thesis.)
\newcommand{\@abstract}{}
\renewcommand{\abstract}[1]{\renewcommand{\@abstract}{#1}}

% DOCVAR: acknowledgements (The text that will be instered into the
% acknowledgments of the thesis.)
\newcommand{\@acknowledgements}{}
\newcommand{\acknowledgements}[1]{\renewcommand{\@acknowledgements}{#1}}

% DOCVAR: affiliation (The name of the thesis writer's college, which will appear
% just below their name.)
\newcommand{\@affiliation}{}
\newcommand{\affiliation}[1]{\renewcommand{\@affiliation}{#1}}

% DOCVAR: affiliation (The name of the thesis writer's college, which will appear
% just below their name.)
\newcommand{\@affiliationlogo}{CollegeShields/CUni}
\newcommand{\affiliationlogo}[1]{\renewcommand{\@affiliationlogo}{#1}}

% DOCVAR: submissionnotice (The submission notice is shown on the bottom of the
% title page.)
\newcommand{\@submissionnotice}{%
This dissertation is submitted in partial fulfilment of the requirements for the degree of Doctor of Philosophy%
}
\newcommand{\submissionnotice}[1]{\renewcommand{\@submissionnotice}{#1}}


%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%
%% Front matter
%%
%% - outside and inside front cover
%% - title leaf
%% Do not include the date of make!
%% Institution + department.
%% Names of referees. (optional)
%% Degree.
%% Date of submission and defense. (optional)
%% Place and date of publication and publishers (and other info by them).
%%%%%
\newcommand{\frontmatter}{

\pagestyle{empty}


%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%
%% Title page components
%%
%%%%%
% The boxes below are all that will be displayed on the title page. They are
% used to calculate exactly how much space should be left between them
% (vertically).

%% LOGO box
\newlength{\simple@logorightnudge}
\setlength{\simple@logorightnudge}{-0.5\paperwidth+12mm}
\newsavebox{\simple@logo}
\begin{lrbox}{\simple@logo}
\hspace*{\simple@logorightnudge}\includegraphics[width=73mm]{\@affiliationlogo}
\end{lrbox}

%% THESIS TITLE box
\newsavebox{\simple@title}
\begin{lrbox}{\simple@title}
  \begin{minipage}[c][\height][c]{.98\textwidth}
    \begin{center}
      \Huge%
      \scshape%
      \@title{}%
    \end{center}
  \end{minipage}
\end{lrbox}

%% AUTHOR&COLLEGE box
\newsavebox{\simple@authorcollege}
\begin{lrbox}{\simple@authorcollege}
    \begin{minipage}[c][\height][c]{.98\textwidth}
        \begin{center}
        {\large \@author{}}\\[1ex]
        \vspace{2cm}
        \@affiliation{}
        \end{center}
    \end{minipage}
\end{lrbox}

%% SUBMISSION NOTICE box
\newsavebox{\simple@submitnotice}
\begin{lrbox}{\simple@submitnotice}
    \begin{minipage}[c][\height][c]{.98\textwidth}
        \begin{center}
            \@submissionnotice{}
        \end{center}
    \end{minipage}
\end{lrbox}

% Now calculate the exact free vertical space
\newlength{\simple@titlepagevspace}
\setlength{\simple@titlepagevspace}{\textheight%
	-\totalheightof{\usebox{\simple@logo}}%
	-\totalheightof{\usebox{\simple@submitnotice}}%
	-\totalheightof{\usebox{\simple@authorcollege}}%
	-\totalheightof{\usebox{\simple@title}}}


%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%
%% Title Page: Put the components (logo, title, author, college and submit
%% notice) onto the title page.
%%
%%%%%
\begin{center}
~\vspace{.02\simple@titlepagevspace}\\
{\usebox{\simple@logo}}\\
\vspace{.28\simple@titlepagevspace}
{\usebox{\simple@title}}\\
\vspace{.23\simple@titlepagevspace}
{\usebox{\simple@authorcollege}}\\
\null\vfill
{\usebox{\simple@submitnotice}}
\end{center}

% Epigraph on odd page. (optional)


%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%
%% Abstract
%%
%%%%%
\chapter*{Abstract}
\thispagestyle{empty}
\@abstract{}


%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%
%% Acknowledgements
%%
%%%%%

\chapter*{Acknowledgements}
\thispagestyle{empty}
\@acknowledgements{}

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%
%% Table of contents, figures, and symbols.
%%
%%%%%

% The following command prevents the page number to be displayed on the first
% page of the TOC.
\addtocontents{toc}{\protect\thispagestyle{empty}}
\pagestyle{empty}
\tableofcontents{}

\cleardoublepage
\pagestyle{plain}
}
