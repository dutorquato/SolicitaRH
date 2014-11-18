/**
 * @license Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.html or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    config.language = 'pt-br';

    // Retira os textos padrao do footer do ckeditor (body p)
    config.toolbar = 'MyToolbar';
    config.removePlugins = 'elementspath';


    // Personaliza butoes no topo do ckeditor (de negrito italico e demais opções)
    config.toolbar = [
       { name: 'document', items: ['Bold', 'Italic', 'Underline', 'RemoveFormat'] },

       { name: 'document', items: ['NumberedList', 'BulletedList'] },

       //{ name: 'tools', items: ['Scayt'] },

       { name: 'tools', items: ['Maximize'] }
    ];

    //config.scayt_autoStartup = true;
    //config.uiColor = '#c0c0c0';
};