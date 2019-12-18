
;(function ($, window, document, undefined) {
	'use strict';

	var $document, $window;
	$window = $(window);
	$document = $(document);
	//Page Load
	function pageLoad() {
		var $body 		= $('body');
		$window.on('beforeunload', function() {
			$body.addClass('amy-fade-out');
		});
	}
	
	//General
	function amyGeneral() {
		$('<div/>', {
			id: 'amy-loading'
		}).appendTo('#main');

		$('#amy-loading').append('<span></span>');
	}

	//Menu
	function amyMenu() {
		var $body 		= $('body'),
			click_event	= document.ontouchstart ? 'touchstart' : 'click',
			$overlay;

		if ($body.find('#amy-menu-overlay').length) {
			$overlay	= $body.find('#amy-menu-overlay');
		} else {
			$overlay	= $('<div id="amy-menu-overlay"></div>').prependTo($body);
		}

		$overlay.on(click_event, function() {
			$body.removeClass('amy-menu-toggle-open');
		});

		$('#amy-menu-toggle').on('click', function(event) {
			event.preventDefault();
			$body.toggleClass('amy-menu-toggle-open');

		});

		if ($(window).width() < amy_script.viewport) {
			$('body').addClass('hasresponsive');
		} else {
			$('body').removeClass('hasresponsive');
		}

	}

	//Tab
	function amyTab() {
		$(document).on('click.bs.tab.data-api', '.bs-tab-nav a', function (e) {
			e.preventDefault();
			$(this).tab('show');
		});
	}

	//Slider
	function amySlider() {
		var as 			= $('.amy-slick');

		if (amy_script.amy_rtl == 1) {
			as.slick({
				rtl: true
			});
		} else {
			as.slick();
		}

	}

	//fancybox
	function amyFancyBox() {
		$('.amy-fancybox').fancybox();
	}

	//Grid Isotope
	function amyGridIsotope() {
		$('.amy-mv-grid .amy-ajax-content').isotope({
			// options
			itemSelector: '.grid-item',
			layoutMode: 'fitRows'
		});
	}

	//Blog Isotope
	function amyBlogIsotope() {
		$('.amy-masonry .page-content').isotope({
			itemSelector: '.entry-item',
			layoutMode: 'masonry'
		});

		$('.amy-blog.amy-grid .row').isotope({
			itemSelector: '.amy-blog.amy-grid .row > div',
		});
	}

	//Movie Filter
	function amyMovieFilter() {
		$('.amy-datepicker').datepicker({
			dateFormat: 'yy-mm-dd',
			onSelect: function(dateText, inst) {
				amyMovieFilterAjax(this);
			}
		});

		$('.select-genre').change(function() {
			amyMovieFilterAjax(this);
		});

		$('.select-cinema').change(function() {
			amyMovieFilterAjax(this);
		});

		$('.amy-mv-sort').change(function() {
			amyMovieFilterAjax(this);
		});
	}

	function amyMovieFilterAjax(el) {
		var $loading 	= $('#amy-loading'),
			$parent		= $(el).parents('.filter-mv'),
			divf		= $parent.siblings('.amy-ajax-content'),
			genreid		= $parent.find('.select-genre').val(),
			cinemaid	= $parent.find('.select-cinema').val(),
			sortby		= $parent.find('.amy-mv-sort').val(),
			release		= $parent.find('.select-date').val(),
			data_send	= $parent.find('.opt-hidden').attr('data-send'),
			string1		= 'genreid=' + genreid + '&cinemaid=' + cinemaid + '&release=' + release + '&sortby=' + sortby,
			string2		= '&data_send=' + data_send,
			string		= string1 + string2;

		$.ajax({
			method: 'POST',
			url: amy_script.ajax_url,
			data: 'action=amy_movie_ajax_filter&' + string,
			dataType: 'json',
			beforeSend: function() {
				$loading.addClass('open');
			},
			success: function(response) {
				$loading.removeClass('open');
				$(divf).empty();
				$(divf).html(response).hide().fadeIn(2000);

				amyBtnShowtime();
			}
		});
	}

	function amyMovieGridDateFilter() {
		$('.amy-date-filter').find('.single-date').each(function() {
			var $this 		= $(this),
				$loading 	= $('#amy-loading'),
				divf		= $this.parents('.amy-date-filter').siblings('.amy-ajax-content'),
				data_send 	= $this.siblings('.option-hidden').find('.opt-hidden').attr('data-send'),
				data_cat	= $this.siblings('.option-hidden').find('.opt-hidden').attr('data-cat'),
				string		= 'release=' + $this.attr('data-value') + '&data_send=' + data_send;

			$this.click(function() {
				$.ajax({
					method: 'POST',
					url: amy_script.ajax_url,
					data: 'action=amy_movie_ajax_filter&' + string,
					dataType: 'json',
					beforeSend: function() {
						$loading.addClass('open');
					},
					success: function(response) {
						$loading.removeClass('open');
						$(divf).empty();
						$(divf).html(response).hide().fadeIn(2000);

						amyBtnShowtime();
					}
				});
			});
		});

		$('.amy-date-filter').find('.amy-calendar').datepicker({
			dateFormat: 'yy-mm-dd',
			onSelect: function(dateText, inst) {
				var $this 		= $(this),
					$loading 	= $('#amy-loading'),
					divf		= $this.parents('.amy-date-filter').siblings('.amy-ajax-content'),
					data_send 	= $this.parents('.single-calendar').siblings('.option-hidden').find('.opt-hidden').attr('data-send'),
					data_cat	= $this.parents('.single-calendar').siblings('.option-hidden').find('.opt-hidden').attr('data-cat'),
					string		= 'release=' + $this.val() + '&data_send=' + data_send;

				$.ajax({
					method: 'POST',
					url: amy_script.ajax_url,
					data: 'action=amy_movie_ajax_filter&' + string,
					dataType: 'json',
					beforeSend: function() {
						$loading.addClass('open');
					},
					success: function(response) {
						$loading.removeClass('open');
						$(divf).empty();
						$(divf).html(response).hide().fadeIn(2000);

						amyBtnShowtime();
					}
				});
			}
		});

		$('.amy-date-filter').find('.change-layout').click(function() {
			var columm,
				$this 		= $(this);

			if ($this.attr('data-column') == 2) {
				columm = 3;
				$this.attr('data-column', 3);
			} else if ($this.attr('data-column') == 3) {
				columm = 2;
				$this.attr('data-column', 2);
			}

			var $loading 	= $('#amy-loading'),
				divf		= $this.parents('.amy-date-filter').siblings('.amy-ajax-content'),
				data_send 	= $this.siblings('.option-hidden').find('.opt-hidden').attr('data-send'),
				data_cat	= $this.siblings('.option-hidden').find('.opt-hidden').attr('data-cat'),
				string		= 'data_send=' + data_send + '&column=' + columm;

			$.ajax({
				method: 'POST',
				url: amy_script.ajax_url,
				data: 'action=amy_movie_ajax_filter&' + string,
				dataType: 'json',
				beforeSend: function() {
					$loading.addClass('open');
				},
				success: function(response) {
					$loading.removeClass('open');
					$(divf).empty();
					$(divf).html(response).hide().fadeIn(2000);

					amyBtnShowtime();
				}
			});
		});
	}

	//Search button
	function amyMovieSearchAction() {
		$('.amy-mv-search .filter-action li').each(function() {
			var $this = $(this),
				value = $this.attr('data-type');

			$this.click(function() {
				$('.amy-mv-search .filter-action li').removeClass('active');
				$this.parents('.filter-action').siblings('.amy_type').val(value);
				$this.addClass('active');
			});
		});
	}

	//Isotope Metro Slider
	function amyMetroSlider() {
		var $slider = $('.amy-isotope');
		$slider.find('.amy-metroslider').isotope({
			itemSelector: '.item',
			layoutMode: 'masonryHorizontal'
		});

		if ($slider.find('.amy-metroslider').length != 0) {
			$slider.smoothDivScroll({
			    touchScrolling: true,
				autoScrollingMode: 'onStart'
			});
		}

	}

	//Rating star
	function amyRatingStar() {
		$('.mv-current-rating').each(function() {
			var point = $(this).attr('data-point');

			$(this).css('width', point);
		});
	}

	//Shortcode Showtime
	function amyAjaxShowtime() {
		$('input[type=radio][name=movie_id]').change(function() {
			amyAjaxShowtimefn($(this), 'movie');
		});

		$('input[type=radio][name=cinema_id]').change(function() {
			amyAjaxShowtimefn($(this), 'cinema');
		});
	}

	//Shortcode showtime ajax
	function amyAjaxShowtimefn(tag, type) {
		var $loading = $('#amy-loading');

		var mvtime		= tag.parents('.amy-mv-showtime'),
			st_type		= mvtime.find('.showtime-type').val(),
			divresult	= mvtime.find('.list-time > div');

		if (type == 'movie') {
			var cinema 		= mvtime.find('input[type=radio][name=cinema_id]:checked'),
				cinema_id 	= cinema.val(),
				movie_id	= tag.val();
		} else if (type == 'cinema') {
			var movie 		= mvtime.find('input[type=radio][name=movie_id]:checked'),
				movie_id 	= movie.val(),
				cinema_id	= tag.val();
		}

		$.ajax({
			method: 'POST',
			url: amy_script.ajax_url,
			data: 'action=amy_movie_ajax_showtime&cinema_id=' + cinema_id + '&movie_id=' + movie_id + '&st_type=' + st_type + '&action_type=shortcode',
			dataType: 'json',
			beforeSend: function() {
				$loading.addClass('open');
			},
			success: function(response) {
				divresult.empty();
				$loading.removeClass('open');
				divresult.html(response);
			}
		});
	}

	//Button showtime
	function amyBtnShowtime() {
		$('.showtime-btn').each(function() {
			var $parent = $(this).parents('.entry-item'),
				$st		= $parent.find('.entry-showtime');

			$(this).click(function() {
				if ($st.hasClass('show')) {
					$st.removeClass('show');
				} else {
					$st.addClass('show');
				}
			});

		});
	}

	//Single Showtime
	function amySingleShowtime() {
		var $loading = $('#amy-loading');

		$('.entry-showtime .select-cinema ul li').each(function() {
			var $this		= $(this),
				cinema_id 	= $(this).attr('data-cinema'),
				movie_id	= $(this).attr('data-movie'),
				showtime	= $('.entry-showtime .showtime');

			$(this).click(function() {
				$('.entry-showtime .select-cinema ul li').removeClass('active');

				$.ajax({
					type: 'POST',
					url: amy_script.ajax_url + '?action=amy_movie_ajax_showtime&cinema_id=' + cinema_id + '&movie_id=' + movie_id,
					dataType: 'json',
					beforeSend: function() {
						$loading.addClass('open');
					},
					success: function(response) {
						showtime.empty();
						$this.addClass('active');
						$loading.removeClass('open');
						showtime.html(response);
					}
				});
			});
		});
	}

	//function amyMovieRate() {
	//	$('.movie-rating-star').each(function() {
	//		$(this).click(function() {
	//			var $loading 	= $('#amy-loading'),
	//				value 		= $(this).attr('data-value'),
	//				m_id		= $(this).attr('data-post');

	//			$.ajax({
	//				method: 'POST',
	//				url: amy_script.ajax_url,
	//				data: 'action=amy_movie_ajax_rate&post_id=' + m_id + '&point=' + value,
	//				dataType: 'json',
	//				beforeSend: function() {
	//					$loading.addClass('open');
	//				},
	//				success: function(response) {
	//					$loading.removeClass('open');

	//					if (response == -1) {
	//						alert(amy_script.amy_rate_already);
	//					} else if (response == 1) {
	//						alert(amy_script.amy_rate_done);
	//						location.reload();
	//					}
	//				}
	//			});
	//		});
	//	});
	//}

	function amyMovieOther() {
		//
		var cp 	= $('.cinema-details'),
			cb	= cp.find('.bg-dl'),
			cg	= cp.find('.cinema-gallery'),
			ci	= cp.find('.cinema-info');

		cb.css('height', cg.height() + ci.height() + 70);
		cg.css('margin-top', '-' + (cg.height() + 100) + 'px');

		//
		var av = $('.amy-mv-list .entry-item'),
			ai = av.find('.entry-thumb img').width(),
			ac = av.find('.entry-content');

		//ac.css('margin-left', ai + 27);
	}

	function V2movietooltip() {
		$('.amy-movie-carousel-1 .amy-movie-items, .amy-movie-grid-1 .amy-movie-items').each(function() {
			var $this, $tooltip, $tooltipstyle;
			$this = $(this);
			$tooltip = $this.find('.tooltip');
			$tooltipstyle = $this.data('tooltip-style');
			$tooltip.tooltipster({
				position: 'right',
				animationDuration: 200,
				delay: 200,
				zIndex: 99,
				theme: 'tooltipster-shadow ' + $tooltipstyle,
				contentCloning: true,
				interactive: true,
				maxWidth: 380,
				minWidth: 300
			});
		});
	}

	function V2movieCarousel1() {
		if ($('.amy-slick-carousel').length) {
			$('.amy-slick-carousel').each(function() {
				var $this;
				$this = $(this);
				$this.imagesLoaded(function() {
					$this.slick();
				});
			});
		}
	}

	function V2movieCarousel2() {
		if ($('.amy-movie-carousel-2').length) {
			$('.amy-movie-carousel-2 .amy-movie-items').each(function() {
				var $arrows, $next, $prev, $this, slick;
				$this = $(this);
				$arrows = $('.slick-arrows');
				$next = $arrows.children(".slick-next");
				$prev = $arrows.children(".slick-prev");
				slick = $this.slick({
					centerMode: true,
					infinite: true,
					centerPadding: '0px',
					slidesToShow: 7,
					dots: false,
					variableWidth: true,
					cssEase: 'linear',
					slideDelay: 200,
					appendArrows: $arrows
				});
				$('.slick-next').on('click', function(e) {
					var i;
					i = $next.index(this);
					slick.eq(i).slick("slickNext");
				});
				$('.slick-prev').on('click', function(e) {
					var i;
					i = $prev.index(this);
					slick.eq(i).slick("slickPrev");
				});
				$this.on('afterChange', function() {});
			});
		}
	}

	function V2movieCarousel3d() {
		$('.amy-movie-carousel-3d .amy-movie-items').each(function() {
			var $this;
			$this = $(this);
			return $this.imagesLoaded(function() {
				var carousel;
				carousel = $this.waterwheelCarousel({
					separation: 190,
					separationMultiplier: 0.5,
					sizeMultiplier: 0.7,
					opacityMultiplier: 1
				});
			});
		});
	}

	function V2trailerlistScrollbar() {
		$('.playlist-trailer').mCustomScrollbar({
			theme: 'minimal-dark',
			scrollInertia: 150
		});
	}

	function V2amyPlay() {
		var $this, newSource, player, poster, src, type, types;
		if ($('.movie-steaming, .amy-movie-trailer-list').length) {
			player = new Plyr('#amyplayer', {
				debug: true,
				title: '',
				iconUrl: amy_script.site_url + '/wp-content/themes/amy-movie/images/icons/plyr.svg',
				keyboard: {
					global: true
				},
				tooltips: {
					controls: true
				},
				captions: {
					active: true
				}
			});
			types = {
				video: 'video',
				audio: 'audio',
				youtube: 'youtube',
				vimeo: 'vimeo'
			};
			newSource = function(type, video, image, autoplay) {
				switch (type) {
					case types.video:
						player.source = {
							type: 'video',
							sources: [{
								src: video,
								type: 'video/mp4'
							}],
							poster: image,
							autoplay: autoplay
						};
						break;
					case types.youtube:
						player.source = {
							type: 'video',
							sources: [{
								src: video,
								provider: 'youtube'
							}],
							autoplay: autoplay
						};
						break;
					case types.vimeo:
						player.source = {
							type: 'video',
							sources: [{
								src: video,
								provider: 'vimeo'
							}],
							autoplay: autoplay
						};
						break;
				}
			};
			if ($('.movie-steaming').length) {
				$this = $('#amyplayer');
				src = $this.data('source');
				type = $this.data('type');
				poster = $this.data('poster');
				newSource(type, src, poster, false);
			}
			$('.amy-movie-trailer-list').each(function() {
				$this = $(this);
				$this.find('.list-item').first().addClass("selected");
				poster = $this.find('.list-item').first().data('poster');
				src = $this.find('.list-item').first().data('source');
				type = $this.find('.list-item').first().data('type');
				$this.find('.trailer-list-wrapper').css('background-image', 'url(' + poster + ')');
				$this.find(".play-video").on('click', function() {
					$('.video-play').hide();
					$('.video-holder').show();
					newSource(type, src, poster, true);
				});
				$('.playlist-trailer .list-item').each(function() {
					$(this).on('click', function() {
						$(".playlist-trailer .list-item").removeClass("selected");
						$(this).addClass("selected");
						poster = $(this).data('poster');
						src = $(this).data('source');
						type = $(this).data('type');
						$(this).closest('.trailer-list-wrapper').css('background-image', 'url(' + poster + ')');
						newSource(type, src, poster, false);
					});
				});
			});
		}
	}

	function V2galleryCarousel() {
		if ($('.amy-gallery-carousel .gallery-list').length) {
			$('.amy-gallery-carousel .gallery-list').each(function() {
				var $this;
				$this = $(this);
				$this.slick({
					dots: true,
					infinite: true,
					speed: 300,
					slidesToShow: 1,
					centerMode: true,
					variableWidth: true
				});
			});
		}
	}

	function V2galleryGrid() {
		$('.amy-gallery-grid').each(function() {
			var $column, $iso, $isoItem, $this;
			$this = $(this);
			$iso = $this.find('.gallery-grid-inner');
			$isoItem = $this.find('.grid-item');
			$column = $iso.data('column');
			$iso.imagesLoaded(function() {
				$iso.isotope({
					animationEngine: 'best-available',
					layoutMode: 'masonry',
					masonry: {
						columnWidth: '.col-md-' + (12 / $column)
					}
				});
				$(window).on('debouncedresize', function() {
					setTimeout(function() {
						$iso.isotope('relayout');
						$(window).resize();
					}, 300);
				});
			});
		});
	}

	function V2movieShowtimeLayout4() {
		$('.amy-movie-showtimews-daily-2').each(function() {
			$(this).find('.amy-movie-item .amy-movie-item-showtimes').each(function() {
				var $this, itemlist, movie_id, selectshowtime;
				$this = $(this);
				selectshowtime = $this.find('.timelist');
				itemlist = $(selectshowtime).siblings('.amy-movie-item-time-list');
				movie_id = selectshowtime.attr('data-movie');
				selectshowtime.on('change', function() {
					return $.ajax({
						method: 'POST',
						url: amy_script.ajax_url,
						dataType: 'json',
						data: 'action=amy_movie_ajax_shortcode_showtime_layout_4&movie_id=' + movie_id + '&date=' + selectshowtime.val(),
						beforeSend: function() {
							$this.addClass('amy-ajax-showtime-loading');
						},
						success: function(response) {
							$this.removeClass('amy-ajax-showtime-loading');
							itemlist.empty();
							itemlist.append(response);
						}
					});
				});
			});
		});
	}

	function V2movieShowtimeLayout3() {
		$('.amy-movie-showtimews-daily-1').each(function() {
			var $that;
			$that = $(this);
			$that.find('.amy-showtimes-header ul li a').each(function() {
				var $this, date, list_movie;
				$this = $(this);
				list_movie = $this.attr('data-movie');
				date = $this.attr('data-date');
				$this.on('click', function(el) {
					el.preventDefault();
					$('.amy-showtimes-header ul li').removeClass('active');
					$this.closest('.amy-showtimes-header ul li').addClass('active');
					return $.ajax({
						method: 'POST',
						url: amy_script.ajax_url,
						dataType: 'json',
						data: 'action=amy_movie_ajax_shortcode_showtime_layout_3&list_movie=' + list_movie + '&date=' + date,
						beforeSend: function() {
							$that.addClass('amy-ajax-showtime-loading');
						},
						success: function(response) {
							var k, v;
							$that.removeClass('amy-ajax-showtime-loading');
							for (k in response) {
								v = response[k];
								$that.find('.amy-movie-item-showtimes.amy-item-' + k).empty();
								$that.find('.amy-movie-item-showtimes.amy-item-' + k).append(v);
							}
						}
					});
				});
			});
		});
	}

	function V2lightbulb() {
		$('.btn_lightbulb').on('click', function() {
			var title;
			$(this).toggleClass("off");
			title = 'Turn off the light';
			if ($(this).hasClass('off')) {
				title = 'Turn on the light';
				$('body').append('<div id="background_lamp"></div>');
			} else {
				$("#background_lamp").remove();
			}
			return $(this).attr('title', title);
		});
	}

	function V2streaming() {
		$('.amy-streaming-link').each(function () {
			var $source = $(this).attr('data-source'),
				$type	= $(this).attr('data-type'),
				$this	= $(this),
				$ms		= $(this).parents('.movie-steaming');

			$this.on('click', function () {
				$ms.find('.amy-streaming-link').removeClass('active');
				$this.addClass('active');
				var player = $ms.find('.amy-movie-item-play .box-player');

				player.empty();
				player.append('<video id="amyplayer" data-source=' + $source + ' data-type=' + $type + ' data-poster="" controls playsinline></video>');

				V2amyPlay();
			})
		})
	}

	$(document).ready(function() {
		pageLoad();
		amyGeneral();
		amyMenu();
		amyTab();
		amySlider();
		amyFancyBox();
		amyRatingStar();
		amyMovieSearchAction();
		//amyBlogIsotope();
		//amyGridIsotope();
		amyBtnShowtime();
		amyMovieFilter();
		amyMovieGridDateFilter();
		amyMetroSlider();
		amyAjaxShowtime();
		amySingleShowtime();
		amyMovieRate();
		amyMovieOther();

		//V2 function
		V2movieCarousel1();
		V2movieCarousel2();
		V2movietooltip();
		V2movieCarousel3d();
		V2trailerlistScrollbar();
		V2amyPlay();
		V2galleryCarousel();
		V2galleryGrid();
		V2movieShowtimeLayout3();
		V2movieShowtimeLayout4();
		V2lightbulb();
		V2streaming();

		$(window).on('resize', function() {
			amyMenu();
		});
	});

})(jQuery, window, document);