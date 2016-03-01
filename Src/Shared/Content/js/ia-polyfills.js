
//Use Modernizr to test for feature support, then use yep/nope to load polyfill scripts


	Modernizr.load({
		//Check for HTML5 Placeholder support
		test: Modernizr.placeholder,
		nope: '../_layouts/15/Akumina.WebParts/js/vendor/placeholders.jquery.min.js',
	});