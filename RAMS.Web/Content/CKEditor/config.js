/**
 * @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    config.toolbarGroups = [
		{ name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
		{ name: 'paragraph', groups: ['indent', 'list', 'blocks', 'align', 'bidi', 'paragraph'] },
		{ name: 'insert', groups: ['insert'] },
		{ name: 'clipboard', groups: ['clipboard', 'undo'] },
		{ name: 'editing', groups: ['find', 'selection', 'spellchecker', 'editing'] },
		{ name: 'links', groups: ['links'] },
		{ name: 'forms', groups: ['forms'] },
		{ name: 'others', groups: ['others'] },
		{ name: 'document', groups: ['mode', 'document', 'doctools'] },
		{ name: 'styles', groups: ['styles'] },
		{ name: 'colors', groups: ['colors'] },
		{ name: 'about', groups: ['about'] },
		{ name: 'tools', groups: ['tools'] }
    ];

    config.removeButtons = 'Underline,Subscript,Superscript,Cut,Copy,Anchor,Image,About,Table,SpecialChar,Blockquote,RemoveFormat,HorizontalRule,Paste,PasteText';
};