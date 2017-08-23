test:
	cd backchannel && source activate backchannel && py.test
	cd server && mix test
	cd server && mix credo

.PHONY: graphics
graphics:
	for file in graphics/*.svg; do \
		inkscape $$file --export-pdf=$${file%.svg}.pdf ; \
	done
