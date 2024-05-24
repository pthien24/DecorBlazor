redirectToCheckout = function (sessionId) {
    var stripe = Stripe("pk_test_51PJ7NmAE5z0wfjvPq4YchOBYkDvEV3T9QCDwam9PDqVV0HjbIE5IhtCvt8MJ1doQFrRbsxLOrAFL8Yah9sIOz9Db00KypmVIOO")
    stripe.redirectToCheckout({ sessionId: sessionId });
}