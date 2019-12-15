
;(function ($, window, document, undefined) {
	'use strict';
	//General
	function amySocialGeneral() {
		//Facebook
		if (amy_user_script.enable_fb_login == 1) {
			window.fbAsyncInit = function () {
				FB.init({
					appId: amy_user_script.fb_app_id, // App ID
					status: true, // check login status
					cookie: true, // enable cookies to allow the server to access the session
					xfbml: true,  // parse XFBML
					version: 'v2.7'
				});
			};

			(function (d, s, id) {
				var js, fjs = d.getElementsByTagName(s)[0];
				if (d.getElementById(id)) {
					return;
				}
				js = d.createElement(s);
				js.id = id;
				js.src = "//connect.facebook.net/en_US/sdk.js";
				fjs.parentNode.insertBefore(js, fjs);
			}(document, 'script', 'facebook-jssdk'));
		}

		//Google
		if (amy_user_script.enable_google_login == 1) {
			(function() {
				var po 	= document.createElement('script'); po.type = 'text/javascript'; po.async = true;
				po.src 	= 'https://apis.google.com/js/client.js?onload=onLoadCallback';
				var s 	= document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);
			})();

			window.onLoadCallback = function() {
				gapi.client.setApiKey(amy_user_script.gg_app_id);
				gapi.client.load('plus', 'v1',function(){});
			}
		}
	}

	//Facebook
	function amyFacebookApi() {
		FB.login(function(response) {
			if (response.status === 'connected') {
				FB.api('/me', function(response) {
					$.ajax({
						url: amy_user_script.ajax_url,
						type: 'POST',
						data: 'action=amy_movie_ajax_social_login&social=fb&info=' + JSON.stringify(response),
						success: function(res) {
							if (typeof(res) === 'string' && res === 'success') {
								location.reload();
							}

							if (typeof(res) === 'string' && res === 'failed') {
								alert('Failed to login, Please refresh the page and try again');
							}
						}
					});
				});
			} else if (response.status === 'not_authorized') {
				FB.login();
			} else {
				FB.login();
			}
		}, {
			scope: 'email,user_about_me',
			return_scopes: true
		});
	}

	//Google
	function amyGoogleApi() {
		gapi.auth.signIn({
			'clientid': 		amy_user_script.gg_client_id,
			'cookiepolicy': 	'single_host_origin',
			'approvalprompt':	'force',
			'scope': 			'https://www.googleapis.com/auth/plus.login https://www.googleapis.com/auth/plus.profile.emails.read',
			'callback': 		function(result) {
				console.log(result);
				if(result['status']['signed_in']) {
					var request = gapi.client.plus.people.get({
						'userId': 'me'
					});

					request.execute(function (resp) {
						var email = '';
						if(resp['emails']) {
							for(i = 0; i < resp['emails'].length; i++) {
								if(resp['emails'][i]['type'] == 'account') {
									email = resp['emails'][i]['value'];
								}
							}

							$.ajax({
								url: amy_user_script.ajax_url,
								type: 'POST',
								data: 'action=amy_movie_ajax_social_login&social=gg&email=' + email,
								success: function(res) {
									if (typeof(res) === 'string' && res === 'success') {
										location.reload();
									}

									if (typeof(res) === 'string' && res === 'failed') {
										alert('Failed to login, Please refresh the page and try again');
									}
								}
							});
						} else {
							alert('Failed to login, Please refresh the page and try again');
						}
					});

				}
			}
		});
	}

	function amySocialLogin() {
		$('.amy-social').each(function() {
			$(this).click(function() {
				var social = $(this).attr('data-social');

				if (social == 'facebook') {
					if (amy_user_script.enable_fb_login == 1) {
						amyFacebookApi();
					}
				} else if (social == 'google') {
					if (amy_user_script.enable_google_login == 1) {
						amyGoogleApi();
					}
				}
			});
		});
	}

	//Ajax submit form
	function amyUserPopupAjaxForm() {
		$('#registerform-popup').submit(function(el) {
			el.preventDefault();

			alert('tsl');
		});
	}

	$(document).ready(function() {
		//fancybox
		$('.amy-user-fancybox').fancybox();
		//function list
		amySocialGeneral();
		amySocialLogin();
	});

})(jQuery, window, document);